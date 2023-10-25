namespace Othello;

internal record struct State(byte WhitePlays, ulong WhiteInfo, byte WhiteCount, ulong BlackInfo, byte BlackCount)
{
    public State Change(ulong play, bool isWhite)
    {
        var state = this with
        {
            WhitePlays = (byte)(WhitePlays ^ 1)
        };

        // TODO: Modify turned pieces
        if (isWhite)
        {
            state.WhiteCount++;
            state.WhiteInfo = play;
        }
        else
        {
            state.BlackCount++;
            state.BlackInfo = play;
        }

        return state;
    }

    public static State Default()
    {

        ulong u = 1;
        ulong whiteInfo = (u << 27) + (u << 36);
        ulong blackInfo = (u << 28) + (u << 35);

        var state = new State(1, whiteInfo, 2, blackInfo, 2);

        return state;
    }

    public override readonly string ToString()
        => $"{WhitePlays} {WhiteInfo} {WhiteCount} {BlackInfo} {BlackCount}";
}