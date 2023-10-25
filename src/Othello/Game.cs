using System.IO;

namespace Othello;

public class Game
{
    private readonly string file;
    private readonly string enemyFile;
    private readonly Tree tree;
    private bool myTurn;

    public Game(string fileName, uint depth)
    {
        myTurn = fileName == "M1";
        file = fileName + ".txt";
        enemyFile = "[OUTPUT]" + fileName + ".txt";
        tree = new Tree(State.Default(), depth);
    }

    public bool Round()
    {
        var state = myTurn ? Play() : EnemyPlay();

        tree.Update(state);

        myTurn = !myTurn;

        return !GameEnded();
    }

    private State Play()
    {
        var state = tree.AlphaBeta();

        GameFile.Create(file, state);

        return state;
    }

    private State EnemyPlay()
    {
        while (!EnemyPlayed()) ;

        return GameFile.Open(enemyFile);
    }

    private bool EnemyPlayed()
        => File.Exists(enemyFile);

    private bool GameEnded()
        => tree.Root.Ended;
}