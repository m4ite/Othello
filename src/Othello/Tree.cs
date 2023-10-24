using System;

namespace Othello;

// TODO
internal class Tree
{
    private readonly uint depth;
    private Node Root;

    public Tree(uint depth)
    {
        this.depth = depth;
        Root = new Node();
    }

    public void AlphaBeta()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }
}