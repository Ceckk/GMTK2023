using System.Collections;
using System.Collections.Generic;

public abstract class Pathfinding
{
    public abstract List<PathNode> FindPath(int startX, int startY, int endX, int endY);
}
