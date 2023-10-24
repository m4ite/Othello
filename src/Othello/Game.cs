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
        // TODO: Create Tree

        if (myTurn)
        {
            ulong u = 1;
            ulong whiteInfo = (u << 27) + (u << 36);
            ulong blackInfo = (u << 28) + (u << 35);
            Board.IsValid(whiteInfo, blackInfo, myTurn);
        }
    }

    public bool Round()
    {
        if (myTurn)
            Play();
        else
            EnemyPlay();

        return GameEnded();
    }

    public void Play()
    {
        // TODO: Get best move and generate State
        var state = new State(0, 0, 0, 0, 0);
        GameFile.Create(file, state);

        myTurn = false;
    }

    public void EnemyPlay()
    {
        while (!EnemyPlayed()) ;

        var state = GameFile.Open(enemyFile);

        myTurn = true;
    }

    private bool EnemyPlayed()
        => File.Exists(enemyFile);

    private bool GameEnded()
        => true;
}