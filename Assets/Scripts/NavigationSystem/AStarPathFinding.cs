using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private PathNode[,] _grid;

    public AStarPathFinding(PathNode[,] grid)
    {
        var width = grid.GetLength(0);
        var height = grid.GetLength(1);
        _grid = new PathNode[width, height];
        // Debug.Log(width);
        // Debug.Log(height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _grid[x, y] = new PathNode(x, y, grid[x, y].isWalkable); ;
            }
        }
    }

    public override List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        var width = _grid.GetLength(0);
        var height = _grid.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PathNode pathNode = _grid[x, y];
                pathNode.g = int.MaxValue;
                pathNode.h = 0;
                pathNode.CalculateF();
                pathNode.visited = false;
                pathNode.cameFromNode = null;
            }
        }

        PathNode startNode = null;
        PathNode endNode = null;
        try
        {
            startNode = _grid[startX, startY];
            endNode = _grid[endX, endY];
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return null;
        }

        var openList = new List<PathNode> { startNode };

        startNode.g = 0;
        startNode.h = CalculateManhattanDistanceCost(startNode, endNode);
        startNode.CalculateF();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestDistanceNode(openList);
            if (currentNode == endNode)
            {
                // Reached final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            currentNode.visited = true;

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (neighbourNode.visited) continue;
                if (!neighbourNode.isWalkable)
                {
                    neighbourNode.visited = true;
                    continue;
                }

                int tentativeGCost = currentNode.g + CalculateManhattanDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.g)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.g = tentativeGCost;
                    neighbourNode.h = CalculateManhattanDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateF();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of nodes on the openList
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        int gridWidth = _grid.GetLength(0);
        int gridHeight = _grid.GetLength(1);

        PathNode left = null;
        PathNode right = null;
        PathNode up = null;
        PathNode down = null;

        // Left
        if (currentNode.x - 1 >= 0)
        {
            left = _grid[currentNode.x - 1, currentNode.y];
            neighbourList.Add(left);
        }
        // Right
        if (currentNode.x + 1 < gridWidth)
        {
            right = _grid[currentNode.x + 1, currentNode.y];
            neighbourList.Add(right);
        }
        // Down
        if (currentNode.y - 1 >= 0)
        {
            down = _grid[currentNode.x, currentNode.y - 1];
            neighbourList.Add(down);
        }
        // Up
        if (currentNode.y + 1 < gridHeight)
        {
            up = _grid[currentNode.x, currentNode.y + 1];
            neighbourList.Add(up);
        }

        // Top-left
        if (up != null && up.isWalkable && left != null && left.isWalkable)
        {
            neighbourList.Add(_grid[currentNode.x - 1, currentNode.y + 1]);
        }
        // Top-right
        if (up != null && up.isWalkable && right != null && right.isWalkable)
        {
            neighbourList.Add(_grid[currentNode.x + 1, currentNode.y + 1]);
        }
        // Bottom-left
        if (down != null && down.isWalkable && left != null && left.isWalkable)
        {
            neighbourList.Add(_grid[currentNode.x - 1, currentNode.y - 1]);
        }
        // Bottom-right
        if (down != null && down.isWalkable && right != null && right.isWalkable)
        {
            neighbourList.Add(_grid[currentNode.x + 1, currentNode.y - 1]);
        }

        return neighbourList;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;
        path.Add(currentNode);

        while (currentNode.cameFromNode != null)
        {
            currentNode = currentNode.cameFromNode;
            path.Add(currentNode);
        }
        path.Reverse();
        return path;
    }

    private int CalculateManhattanDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestDistanceNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].f < lowestFCostNode.f)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

}
