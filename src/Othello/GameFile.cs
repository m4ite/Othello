using System.IO;

namespace Othello;

internal static class GameFile
{
    public static bool Exists(string file)
        => File.Exists(file);

    public static void Create(string file, State board)
    {
        using StreamWriter sw = File.CreateText(file);
        sw.WriteLine(board.ToString());
    }

    public static State Open(string file)
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
}