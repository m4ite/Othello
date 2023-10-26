using System;

namespace Othello;

internal record struct Play(ulong Board, byte Count, bool Played)
{
    private const ulong SHIFT = 1;
    private const int HORIZONTAL_DIFF = 8;
    private const int VERTICAL_DIFF = -1;
    private const int SIDE = 8;
    private const int AROUND_SQUARES = 8;
    private const int BOARD_SIZE = 64;

    public static Play[] GetPlays(State state)
    {
        var isWhite = state.WhitePlays == 1;
        var myBoard = isWhite ? state.WhiteInfo : state.BlackInfo;
        var enemyBoard = isWhite ? state.BlackInfo : state.WhiteInfo;

        var n = (isWhite ? state.BlackCount : state.WhiteCount) * AROUND_SQUARES;
        var plays = new Play[n];
        var index = 0;

        for (int i = 0; i < BOARD_SIZE; i++)
        {
            var bit = GetBit(enemyBoard, i);

            // Check is has an enemy piece
            if (bit == 0)
                continue;

            GetAdjacent(myBoard, enemyBoard, plays, i, ref index);
        }

        var possibilities = new Play[index + 1];
        Array.Copy(plays, possibilities, index);
        possibilities[index] = new Play(myBoard, 0, false);

        return possibilities;
    }

    private static void GetAdjacent(ulong myBoard, ulong enemyBoard, Play[] plays, int emptyPosition, ref int index)
    {
        var board = myBoard | enemyBoard;

        for (int i = -1; i < 2; i++)
        {
            var y = emptyPosition / SIDE + i;
            if (y < 0 || y > 7)
                break;

            for (int j = -1; j < 2; j++)
            {
                var x = emptyPosition % SIDE + j;
                if (x < 0 || x > 7)
                    break;

                var boardPosition = emptyPosition + VERTICAL_DIFF * i + HORIZONTAL_DIFF * j;
                if (boardPosition < 0 || boardPosition >= BOARD_SIZE)
                    break;

                // Check if has a piece in this position
                if (GetBit(board, boardPosition) == 1)
                    continue;

                var play = GetPlay(myBoard, enemyBoard, boardPosition);
                if (play.Count == 0)
                    continue;

                // Add a piece in this position
                play.Board |= SHIFT << boardPosition;
                plays[index++] = play;
            }
        }
    }

    private static Play GetPlay(ulong myBoard, ulong enemyBoard, int playPosition)
    {
        ulong newBoard = myBoard;
        byte turned = 0;

        for (int i = -1; i < 2; i++)
        {
            var y = playPosition / SIDE + i;
            if (y < 0 || y > 7)
                break;

            for (int j = -1; j < 2; j++)
            {
                var x = playPosition % SIDE + j;
                if (x < 0 || x > 7)
                    break;

                byte turnedCount = 0;
                ulong tempBoard = 0;
                var wasTurned = false;

                for (int k = 1; k < 8; k++)
                {
                    var boardPosition = playPosition + VERTICAL_DIFF * i + HORIZONTAL_DIFF * j * k;
                    if (boardPosition < 0 || boardPosition >= BOARD_SIZE)
                        break;

                    if (wasTurned)
                        break;

                    // Turn pieces
                    if (GetBit(myBoard, boardPosition) == 1)
                    {
                        turned += turnedCount;
                        newBoard |= tempBoard;
                        wasTurned = true;
                    }
                    
                    // Add enemy piece to stage
                    if (GetBit(enemyBoard, boardPosition) == 1)
                    {
                        tempBoard |= SHIFT << boardPosition;
                        turnedCount++;
                    }
                }
            }
        }

        return new Play(newBoard, turned, true);
    }

    private static ulong GetBit(ulong data, int index)
        => (data >> index) & 1;
}