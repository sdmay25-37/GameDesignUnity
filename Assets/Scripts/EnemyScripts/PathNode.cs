using UnityEngine;

public class PathNode
{
    public Vector2 position;
    public float gCost = float.MaxValue; // Initialize with high values to ensure they are updated correctly
    public float hCost = float.MaxValue;
    public PathNode parent;
    public bool isWalkable = true;

    public float fCost => gCost + hCost;

    // Constructor to initialize position
    public PathNode(Vector2 position)
    {
        this.position = position;
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
