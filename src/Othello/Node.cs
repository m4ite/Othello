using System;

namespace Othello;

internal class Node
{
    private Node[] children = Array.Empty<Node>();
    public readonly State State;
    public bool Ended { get; private set; }

    public Node(State state)
        => State = state;

    public void GenChildren(uint depth)
    {
        if (depth == 0 || Ended)
            return;

        if (this.children.Length != 0)
        {
            foreach (var child in this.children)
                child.GenChildren(depth - 1);
        }

        var plays = Board.GetPlays(State);
        var children = new Node[plays.Length];

        for (int i = 0; i < children.Length; i++)
        {
            var state = State.Change(plays[i], State.WhitePlays == 1);
            children[i] = new Node(state);
        }

        if (children.Length == 0)
        {
            Ended = true;
            return;
        }

        this.children = children;

        foreach (var child in children)
            child.GenChildren(depth - 1);
    }

    public Node GetChild(State state)
    {
        foreach (var child in children)
        {
            if (child.State == state)
                return this;
        }

        throw new Exception("Child not found");
    }
}