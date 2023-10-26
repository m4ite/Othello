using System;

namespace Othello;

internal class Node
{
    private Node[] children = Array.Empty<Node>();
    public float Value { get; private set; }
    public bool Ended { get; private set; }
    private bool hasChildren;
    public readonly State State;

    public Node(State state)
        => State = state;

    public void GenChildren(uint depth)
    {
        if (depth == 0 || Ended)
            return;

        if (!hasChildren)
            GenerateChildren();

        foreach (var child in children)
            child.GenChildren(depth - 1);
    }

    private void GenerateChildren()
    {
        hasChildren = true;

        var plays = Play.GetPlays(State);
        if (plays.Length == 0)
        {
            Ended = true;
            return;
        }

        var children = new Node[plays.Length];
        var isWhite = State.WhitePlays == 1;

        for (int i = 0; i < children.Length; i++)
        {
            var state = State.Change(plays[i], isWhite);
            children[i] = new Node(state);
        }

        this.children = children;
    }

    public Node GetChild(State state)
    {
        foreach (var child in children)
        {
            if (child.State == state)
                return child;
        }
        
        // TODO: Create custom error
        throw new Exception("Child not found");
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

    private float Heuristic(bool ImWhite)
    {
        if (!Ended)
            return 0f;

        if (ImWhite)
            return State.WhiteCount > State.BlackCount ? float.PositiveInfinity : float.NegativeInfinity;

        return State.BlackCount > State.WhiteCount ? float.PositiveInfinity : float.NegativeInfinity;
    }

    public void AlphaBeta(uint depth, bool ImWhite)
        => AlphaBeta(float.NegativeInfinity, float.PositiveInfinity, true, depth, ImWhite);

    private float AlphaBeta(float alfa, float beta, bool maximize, uint depth, bool ImWhite)
    {
        if (depth == 0 || Ended)
        {
            Value = Heuristic(ImWhite);
            return Value;
        }

        Value = maximize ? float.NegativeInfinity : float.PositiveInfinity;


        foreach (var child in children)
        {
            var alphaBeta = child.AlphaBeta(alfa, beta, !maximize, depth - 1, ImWhite);

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