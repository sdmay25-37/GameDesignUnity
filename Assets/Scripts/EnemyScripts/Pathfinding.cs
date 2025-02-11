using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private int maxIterations = 10000; // Depending on how the game runs this can be changed NOTE WAS 3000
    private float tolerance = 1.0f; // Starting tolerance, will progressively increase
    private int visitThreshold = 3; // Maximum times a node can be revisited to prevent oscillation

    public List<PathNode> FindPath(Vector2 start, Vector2 target, List<LightArea> lightAreas = null)
    {
        // Debug.Log("Pathfinding started from " + start + " to " + target);

        PriorityQueue<PathNode> openList = new PriorityQueue<PathNode>();
        HashSet<PathNode> closedList = new HashSet<PathNode>();
        Dictionary<PathNode, int> visitedNodes = new Dictionary<PathNode, int>();

        PathNode startNode = new PathNode(start) { gCost = 0, hCost = ManhattanDistance(start, target) };
        PathNode targetNode = new PathNode(target);

        openList.Enqueue(startNode);

        int iterationCount = 0;

        while (openList.Count > 0 && iterationCount < maxIterations)
        {
            if (iterationCount >= maxIterations)
            {
                // Signal failure to find a path
                Debug.LogWarning("Pathfinding exceeded max iterations. Returning null.");
                return null;
            }

            iterationCount++;
            PathNode currentNode = openList.Dequeue();

            // Debug.Log($"Current Node Position: {currentNode.position} | fCost: {currentNode.fCost} | gCost: {currentNode.gCost} | hCost: {currentNode.hCost}");

            closedList.Add(currentNode);

            // Track visit count for each node
            if (visitedNodes.ContainsKey(currentNode))
            {
                visitedNodes[currentNode]++;
            }
            else
            {
                visitedNodes[currentNode] = 1;
            }

            // Stop if the node has been visited more than the threshold
            if (visitedNodes[currentNode] > visitThreshold)
            {
                // Debug.Log($"Node {currentNode.position} exceeded visit threshold. Adding to closed list to avoid looping.");
                continue;
            }

            // Check if close enough to the target
            if (Vector2.Distance(currentNode.position, targetNode.position) <= tolerance)
            {
                // Debug.Log("Target node reached within tolerance. Path found.");
                return RetracePath(startNode, currentNode);
            }

            List<PathNode> neighbors = GetNeighbors(currentNode, lightAreas);
            // Debug.Log("Number of neighbors evaluated: " + neighbors.Count);

            foreach (var neighbor in neighbors)
            {
                if (closedList.Contains(neighbor) || !neighbor.isWalkable)
                    continue;

                float tentativeGCost = currentNode.gCost + Vector2.Distance(currentNode.position, neighbor.position);

                if (tentativeGCost < neighbor.gCost)
                {
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = ManhattanDistance(neighbor.position, targetNode.position);
                    neighbor.parent = currentNode;

                    if (!visitedNodes.ContainsKey(neighbor))
                    {
                        openList.Enqueue(neighbor);
                    }
                }
            }
        }

        // Debug.LogWarning("No path found within max iterations or all nodes processed.");
        return null;
    }

    private List<PathNode> GetNeighbors(PathNode node, List<LightArea> lightAreas)
    {
        List<PathNode> neighbors = new List<PathNode>();
        Vector2[] directions =
        {
            Vector2.up * 3, Vector2.down * 3, Vector2.left * 3, Vector2.right * 3,
            (Vector2.up + Vector2.right).normalized * 3,
            (Vector2.up + Vector2.left).normalized * 3,
            (Vector2.down + Vector2.right).normalized * 3,
            (Vector2.down + Vector2.left).normalized * 3
        };

        foreach (Vector2 direction in directions)
        {
            Vector2 neighborPos = node.position + direction;
            bool isWalkable = true;

            // Check if the neighbor position is within any light area's buffer zone
            if (lightAreas != null)
            {
                foreach (LightArea light in lightAreas)
                {
                    float bufferRadius = light.radius + 1.0f; // Adjust buffer size to avoid skimming edges
                    if (Vector2.Distance(neighborPos, light.transform.position) < bufferRadius)
                    {
                        isWalkable = false;
                        // Debug.Log($"Neighbor at {neighborPos} is within light area at {light.transform.position} and marked as non-walkable.");
                        break;
                    }
                }
            }

            PathNode neighborNode = new PathNode(neighborPos) { isWalkable = isWalkable, gCost = float.MaxValue, hCost = 0 };
            // Debug.Log($"Evaluating Neighbor Position: {neighborPos} | Walkable: {isWalkable}");
            neighbors.Add(neighborNode);
        }

        return neighbors;
    }

    private float ManhattanDistance(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    public List<Vector2> SmoothPath(List<PathNode> rawPath)
    {
        List<Vector2> smoothedPath = new List<Vector2>();

        for (int i = 0; i < rawPath.Count - 1; i++)
        {
            Vector2 currentPoint = rawPath[i].position;
            Vector2 nextPoint = rawPath[i + 1].position;

            smoothedPath.Add(currentPoint);
            Vector2 midpoint = (currentPoint + nextPoint) / 2;
            smoothedPath.Add(midpoint);
        }

        smoothedPath.Add(rawPath[rawPath.Count - 1].position);
        return smoothedPath;
    }
}
