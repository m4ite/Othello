using System;
using Othello;

const uint DEPTH = 4;

if (args.Length != 1)
    throw new Exception("Invalid arguments");

string fileName = args[0];

var game = new Game(fileName, DEPTH);

while (game.Round()) ;

Console.WriteLine("Game ended");