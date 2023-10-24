namespace Othello;

internal static class Board
{
    public static bool IsValid(ulong board)
    {
        // TODO: Check if board is valid
        return board != ulong.MaxValue;
    }
}