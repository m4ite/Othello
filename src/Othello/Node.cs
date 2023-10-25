using System;

namespace Othello;

internal class Node
{
    private Node[] children = Array.Empty<Node>();
    public float Value { get; private set; }
    public bool Ended { get; private set; }
    public readonly State State;

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


    private float Heuristic(bool myTurn)
    {
        if (Ended)
            return myTurn ? float.NegativeInfinity : float.PositiveInfinity;


        return 0f;
    }

    public State GetBestChild()
    {
        var state = State;
        var value = float.NegativeInfinity;

        foreach (var child in children)
        {
            if (value < child.Value)
            {
                value = child.Value;
                state = child.State;
            }
        }

        return state;
    }

    public void AlphaBeta(uint depth)
        => AlphaBeta(float.NegativeInfinity, float.PositiveInfinity, true, depth);

    private float AlphaBeta(float alfa, float beta, bool maximize, uint depth)
    {
        if (depth == 0 || Ended)
        {
            Value = Heuristic(maximize);
            return Value;
        }

        Value = maximize ? float.NegativeInfinity : float.PositiveInfinity;


        foreach (var child in children)
        {
            var alphaBeta = child.AlphaBeta(alfa, beta, !maximize, depth - 1);

            if (maximize)
            {
                Value = float.Max(Value, alphaBeta);

                if (alphaBeta > beta)
                    break;

                alfa = float.Max(alfa, Value);
            }
            else
            {
                Value = float.Min(Value, alphaBeta);

                if (alphaBeta < alfa)
                    break;

                beta = float.Min(beta, Value);
            }
        }

        return Value;
    }
}