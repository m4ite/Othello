using System;
using System.Collections.Generic;

namespace Othello;

internal static class Board
{
    const int HorizontalDiff = 8;
    const int VerticalDiff = -1;

    public static bool IsValid(ulong whiteBoard, ulong blackBoard, bool isWhite)
    {
        var board = whiteBoard | blackBoard;
        var emptySquares = ~board;

        var possibilities = isWhite ?
            GetPossibilities(blackBoard, emptySquares, whiteBoard, blackBoard) :
            GetPossibilities(whiteBoard, emptySquares, whiteBoard, blackBoard);

        foreach (var item in possibilities)
            Console.Write($"{item}, ");

        return true;
    }

    private static int[] GetPossibilities(ulong board, ulong emptySquares, ulong whiteBoard, ulong blackBoard)
    {
        var possibilities = new List<int>();
        for (int index = 0; index < 64; index++)
        {
            var bit = CheckBit(board, index);

            if (!bit)
                continue;

            GetAdjacent(emptySquares, possibilities, index, whiteBoard, blackBoard);
        }

        return possibilities.ToArray();
    }

    private static void GetAdjacent(ulong emptySquares, List<int> possibilities, int index, ulong whiteBoard, ulong blackBoard)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var checkIndex = index + HorizontalDiff * i + VerticalDiff * j;

                if (!CheckBit(emptySquares, checkIndex))
                    continue;

                if (IsValidPlay(checkIndex, whiteBoard, blackBoard))
                    possibilities.Add(checkIndex);
            }
        }
    }

    private static bool IsValidPlay(int index, ulong myBoard, ulong enemyBoard)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var flag = false;
                for (int k = 1; k < 8; k++)
                {
                    var checkIndex = index + HorizontalDiff * i * k + VerticalDiff * j * k;

                    if (checkIndex > 64 || checkIndex < 0)
                        break;

                    if (CheckBit(myBoard, checkIndex))
                    {
                        if (flag)
                            return true;

                        break;
                    }

                    if (!flag && CheckBit(enemyBoard, checkIndex))
                        flag = true;
                }
            }
        }

        return false;
    }

    private static bool CheckBit(ulong data, int index)
        => ((data >> index) & 1) == 1;
}