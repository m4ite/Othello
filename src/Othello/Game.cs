using System.IO;
using System.Threading;

namespace Othello;

public class Game
{
    private readonly string myFile;
    private readonly string enemyFile;
    private readonly Tree tree;
    private readonly bool ImWhite;
    private bool myTurn;

    public Game(string fileName, uint depth)
    {
        ImWhite = fileName == "m1";
        myTurn = ImWhite;
        myFile = fileName + ".txt";
        enemyFile = "[OUTPUT]" + fileName + ".txt";
        tree = new Tree(depth);
    }

    public bool Round()
    {
        Thread.Sleep(3000);
        var state = myTurn ? Play() : EnemyPlay();

        tree.Update(state);

        myTurn = !myTurn;

        return !GameEnded();
    }

    private State Play()
    {
        var state = tree.GetBestPlay(ImWhite);

        state.Save(myFile, tree.Root.State);

        return state;
    }

    private State EnemyPlay()
    {
        while (!EnemyPlayed())
            Thread.Sleep(1000);

        var state = State.Load(enemyFile);

        return state ?? tree.Root.State with {
            WhitePlays = (byte)(tree.Root.State.WhitePlays ^ 1)
        };
    }

    private bool EnemyPlayed()
        => File.Exists(enemyFile);

    private bool GameEnded()
        => tree.Root.Ended;
}