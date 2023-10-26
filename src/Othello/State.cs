using System.IO;

namespace Othello;

internal readonly record struct State(byte WhitePlays, ulong WhiteInfo, byte WhiteCount, ulong BlackInfo, byte BlackCount)
{
    public static State Load(string file)
    {
        var line = File
            .ReadAllText(file)
            .Split(' ');

        File.Delete(file);

        var board = new State(
            byte.Parse(line[0]),
            ulong.Parse(line[1]),
            byte.Parse(line[2]),
            ulong.Parse(line[3]),
            byte.Parse(line[4])
        );

        return board;
    }

    public readonly void Save(string file, State prevState)
    {
        string content = prevState == this ? "pass" : ToString();
        File.WriteAllText(file, content);
    }

    public static State Default()
    {

        ulong u = 1;
        ulong whiteInfo = (u << 27) + (u << 36);
        ulong blackInfo = (u << 28) + (u << 35);

        var state = new State(1, whiteInfo, 2, blackInfo, 2);

        return state;
    }

    public State Change(PlayData data, bool isWhite)
    {
        var whiteCount = WhiteCount;
        var blackCount = BlackCount;

        if (isWhite)
        {
            whiteCount += data.Count;
            blackCount -= data.Count;

            if (data.Played)
                whiteCount++;
        }
        else
        {
            whiteCount -= data.Count;
            blackCount += data.Count;

            if (data.Played)
                blackCount++;
        }

        var state = this with
        {
            WhitePlays = (byte)(WhitePlays ^ 1),
            WhiteInfo = isWhite ? data.Play : (WhiteInfo & data.Play) ^ WhiteInfo,
            WhiteCount = whiteCount,
            BlackInfo = isWhite ? (BlackInfo & data.Play) ^ BlackInfo : data.Play,
            BlackCount = blackCount

        };

        return state;
    }

    public override readonly string ToString()
        => $"{WhitePlays} {WhiteInfo} {WhiteCount} {BlackInfo} {BlackCount}";
}