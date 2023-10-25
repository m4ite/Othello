namespace Othello;

internal record struct State(byte WhitePlays, ulong WhiteInfo, byte WhiteCount, ulong BlackInfo, byte BlackCount)
{
    public State Change(Data data, bool isWhite)
    {
        var state = this with
        {
            WhitePlays = (byte)(WhitePlays ^ 1)
        };

        if (isWhite)
        {
            state.WhiteInfo = data.Play;
            state.BlackInfo = (BlackInfo & data.Play) ^ BlackInfo;

            state.WhiteCount += data.Count;
            state.BlackCount -= data.Count;
            
            if (data.Played)
                state.WhiteCount++;
        }
        else
        {
            state.BlackInfo = data.Play;
            state.WhiteInfo = (WhiteInfo & data.Play) ^ WhiteInfo;

            state.BlackCount += data.Count;
            state.WhiteCount -= data.Count;

            if (data.Played)
                state.BlackCount++;
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