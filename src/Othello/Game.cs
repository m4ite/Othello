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

        var state = myTurn ? State.Default() : GameFile.Open(enemyFile);
        tree = new Tree(state, depth);
    }

    public bool Round()
    {
        if (myTurn)
            Play();
        else
            EnemyPlay();

        return !GameEnded();
    }

    private void Play()
    {
        var state = tree.AlphaBeta();

        GameFile.Create(file, state);

        myTurn = false;
    }

    private void EnemyPlay()
    {
        while (!EnemyPlayed()) ;

        var state = GameFile.Open(enemyFile);

        tree.Update(state);

        myTurn = true;
    }

    private bool EnemyPlayed()
        => File.Exists(enemyFile);

    private bool GameEnded()
        => Board.GetPlays(tree.Root.State).Length == 0;
}