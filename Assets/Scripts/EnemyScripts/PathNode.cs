using System;
using UnityEngine;

public class PathNode : IComparable<PathNode>
{
    public Vector2 position;
    public float gCost = float.MaxValue;
    public float hCost = float.MaxValue;
    public PathNode parent;
    public bool isWalkable = true;

    public float fCost => gCost + hCost;

    public PathNode(Vector2 position)
    {
        this.position = position;
    }

    public int CompareTo(PathNode other)
    {
        return fCost.CompareTo(other.fCost);
    }

    public override bool Equals(object obj)
    {
        if (obj is PathNode node)
        {
            return position == node.position;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return position.GetHashCode();
    }
}
