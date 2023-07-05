public class PathNode
{
    public int x;
    public int y;

    public int g;
    public int h;
    public int f;

    public bool isWalkable;
    public bool visited;
    public PathNode cameFromNode;

    public PathNode(int x, int y, bool isWalkable)
    {
        this.x = x;
        this.y = y;
        this.isWalkable = isWalkable;
    }

    public void CalculateF()
    {
        f = g + h;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}