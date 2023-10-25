namespace Othello;

internal class Tree
{
    public Node Root { get; private set; }
    private readonly uint depth;

    public Tree(State state, uint depth)
    {
        this.depth = depth;
        Root = new Node(state);
        Root.GenChildren(depth);
    }

    public State GetBestPlay()
    {
        Root.AlphaBeta(depth);
        return Root.GetBestChild();
    }

    public void Update(State state)
    {
        Root = Root.GetChild(state);
        Root.GenChildren(depth);
    }
}