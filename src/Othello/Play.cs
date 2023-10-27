using System;

namespace Othello;

internal record struct Play(ulong Board, byte Count)
{
    private const ulong SHIFT = 1;
    private const int HORIZONTAL_DIFF = 8;
    private const int VERTICAL_DIFF = 1;
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

        var adjacent = GetAdjacent(myBoard, enemyBoard);
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            var bit = GetBit(adjacent, i);

            // Check is has an adjacent play
            if (bit == 0)
                continue;

            var play = GetPlay(myBoard, enemyBoard, i);

            // Check if it is a valid play
            if (play.Count == 0)
                continue;

            // Add a piece in this position
            play.Board |= SHIFT << i;
            plays[index++] = play;
        }

        var possibilities = new Play[index + 1];
        Array.Copy(plays, possibilities, index);
        possibilities[index] = new Play(myBoard, 0);

        return possibilities;
    }

    private static ulong GetAdjacent(ulong myBoard, ulong enemyBoard)
    {
        ulong adjacent = 0;
        adjacent |= enemyBoard >> VERTICAL_DIFF; // up
        adjacent |= enemyBoard << HORIZONTAL_DIFF; // right
        adjacent |= enemyBoard >> HORIZONTAL_DIFF; // left
        adjacent |= enemyBoard << VERTICAL_DIFF; // down
        adjacent |= enemyBoard << HORIZONTAL_DIFF + VERTICAL_DIFF; // right | down
        adjacent |= enemyBoard << HORIZONTAL_DIFF - VERTICAL_DIFF; // right | up
        adjacent |= enemyBoard >> HORIZONTAL_DIFF + VERTICAL_DIFF; // left | up
        adjacent |= enemyBoard >> HORIZONTAL_DIFF - VERTICAL_DIFF; // left | down
        adjacent ^= myBoard | enemyBoard; // Remove used squares

        return adjacent;
    }

    private static Play GetPlay(ulong myBoard, ulong enemyBoard, int index)
    {
        ulong newBoard = myBoard;
        byte turned = 0;

        MoveAround((i, j) =>
        {
            byte turnedCount = 0;
            ulong tempBoard = 0;

            for (int k = 1; k < SIDE; k++)
            {
                var y = index / SIDE + i * k;
                if (y < 0 || y > 7)
                    break;

                var x = index % SIDE + j * k;
                if (x < 0 || x > 7)
                    break;

                var position = index + VERTICAL_DIFF * i * k + HORIZONTAL_DIFF * j * k;
                if (position < 0 || position >= BOARD_SIZE)
                    break;

                // Turn pieces
                if (GetBit(myBoard, position) == 1)
                {
                    turned += turnedCount;
                    newBoard |= tempBoard;
                    break;
                }

                // Add enemy piece to stage
                if (GetBit(enemyBoard, position) == 1)
                {
                    tempBoard |= SHIFT << position;
                    turnedCount++;
                    continue;
                }

                break;
            }
        });

        return new Play(newBoard, turned);
    }

    private static void MoveAround(Action<int, int> func)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
                func(i, j);
        }
    }

    private static ulong GetBit(ulong data, int index)
        => (data >> index) & 1;
}