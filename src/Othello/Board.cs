using System;
namespace Othello;

internal static class Board
{
    private const ulong SHIFT = 1;
    private const int HORIZONTAL_DIFF = 8;
    private const int VERTICAL_DIFF = -1;
    private const int SIDE = 8;

    public static PlayData[] GetPlays(State state)
    {
        var isWhite = state.WhitePlays == 1;

        // Board data
        var board = state.WhiteInfo | state.BlackInfo;
        var emptySquares = ~board;

        // Boards
        var myBoard = isWhite ? state.WhiteInfo : state.BlackInfo;
        var enemyBoard = isWhite ? state.BlackInfo : state.WhiteInfo;

        // Plays array
        var n = (isWhite ? state.BlackCount : state.WhiteCount) * 8;
        var plays = new PlayData[n];
        var index = 0;

        // Get all valid plays
        for (int i = 0; i < 64; i++)
        {
            var bit = CheckBit(enemyBoard, i);

            if (!bit)
                continue;

            GetAdjacent(myBoard, enemyBoard, emptySquares, plays, i, ref index);
        }

        // New array with the right length of plays
        var possibilities = new PlayData[index + 1];
        Array.Copy(plays, possibilities, index);

        // Pass play
        possibilities[index] = new PlayData(myBoard, 0, false);

        return possibilities;
    }

    private static void GetAdjacent(ulong myBoard, ulong enemyBoard, ulong emptySquares, PlayData[] plays, int square, ref int playIndex)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var index = square + HORIZONTAL_DIFF * i + VERTICAL_DIFF * j;
                var x = square % SIDE + i;
                var y = square / SIDE + j;

                if (x >= SIDE || x < 0 || y >= SIDE || y < 0)
                    continue;

                if (!CheckBit(emptySquares, index))
                    continue;

                var data = GetData(myBoard, enemyBoard, index);
                if (data.Count > 0)
                {
                    data.Play |= SHIFT << index;
                    plays[playIndex++] = data;
                }
            }
        }
    }

    private static PlayData GetData(ulong myBoard, ulong enemyBoard, int square)
    {
        byte turned = 0;
        ulong newBoard = myBoard;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var flag = false;
                byte count = 0;

                for (int k = 1; k < 8; k++)
                {
                    var index = square + HORIZONTAL_DIFF * i * k + VERTICAL_DIFF * j * k;

                    if (index > 64 || index < 0)
                        break;

                    if (flag)
                        break;

                    if (CheckBit(myBoard, index))
                    {
                        turned += count;
                        flag = true;
                    }

                    if (CheckBit(enemyBoard, index))
                    {
                        newBoard |= SHIFT << index;
                        count++;
                    }
                }
            }
        }

        return new PlayData(newBoard, turned, true);
    }

    private static bool CheckBit(ulong data, int index)
        => ((data >> index) & 1) == 1;
}
