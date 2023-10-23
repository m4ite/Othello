namespace Othello;

internal record struct Board(byte WhitePlays, ulong WhiteInfo, byte WhiteCount, ulong BlackInfo, byte BlackCount)
{
    public override readonly string ToString()
        => $"{WhitePlays} {WhiteInfo} {WhiteCount} {BlackInfo} {BlackCount}";

}