namespace Othello;

internal class Tree
{
    public Node Root { get; private set; }
    private readonly uint depth;

    public Tree(uint depth)
    {
        this.depth = depth;
        Root = new Node(State.Default());
        Root.GenChildren(depth);
    }

    public State GetBestPlay(bool ImWhite)
    {
        Root.AlphaBeta(depth, ImWhite);
        return Root.GetBestChild();
    }

    public void Update(State state)
    {
        Root = Root.GetChild(state);
        Root.GenChildren(depth);
    }
}