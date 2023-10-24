using System;
using System.Threading;
using Othello;

internal class Program
{
    const uint DEPTH = 4;

    private static void Main(string[] args)
    {

        if (args.Length != 1)
            throw new Exception("Invalid arguments");

        string fileName = args[0];

        var game = new Game(fileName, DEPTH);
        var running = true;

        while (running)
        {
            Thread.Sleep(1000);
            running = game.Round();
        }

        Console.WriteLine("Game ended");
    }
}
