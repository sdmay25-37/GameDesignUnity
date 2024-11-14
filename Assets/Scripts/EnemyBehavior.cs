using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public Transform[] lightSources;
    public float lightRadius = 2f;
    public float moveSpeed = 2f;
    public float avoidanceOffset = 1f; // Offset distance to avoid light edges

    void Update()
    {
        Vector2 directionToPlayer = ((Vector2)player.position - (Vector2)transform.position).normalized;
        Vector2 targetPosition = (Vector2)player.position;

        // Check if there's a clear path to the player
        if (IsPathClear((Vector2)transform.position, targetPosition))
        {
            // Move directly to the player if path is clear
            MoveTowards(targetPosition);
        }
        else
        {
            // Try to find the closest valid position near the player
            Vector2 alternativeTarget = FindAlternativeTarget(directionToPlayer);

            // If an alternative path is found, move towards it; otherwise, stop
            if (alternativeTarget != (Vector2)transform.position)
            {
                MoveTowards(alternativeTarget);
            }
        }
    }

    // Checks if there is a clear path from the start position to the target
    bool IsPathClear(Vector2 start, Vector2 target)
    {
        foreach (var light in lightSources)
        {
            float distanceToLight = Vector2.Distance(start, (Vector2)light.position);
            float distanceToTarget = Vector2.Distance(target, (Vector2)light.position);

            // If the path intersects with a light's radius, return false
            if (distanceToLight < lightRadius || distanceToTarget < lightRadius)
            {
                Vector2 directionToTarget = (target - start).normalized;
                Vector2 directionToLight = ((Vector2)light.position - start).normalized;

                // Check if the light is in the direct path
                if (Vector2.Dot(directionToTarget, directionToLight) > 0.8f) // Threshold to check if in path
                {
                    return false;
                }
            }
        }
        return true;
    }

    // Finds an alternative target position near the player, avoiding lights
    Vector2 FindAlternativeTarget(Vector2 originalDirection)
    {
        float angleStep = 30f; // Angle increment to test alternative directions
        int attempts = 12; // Number of alternative directions to try (360 / 30 = 12)

        for (int i = 1; i <= attempts; i++)
        {
            // Calculate alternative directions by rotating around the player
            float angle = angleStep * i;
            Vector2 rotatedDirection = Quaternion.Euler(0, 0, angle) * originalDirection;
            Vector2 potentialTarget = (Vector2)transform.position + rotatedDirection * moveSpeed * Time.deltaTime;

            if (IsPathClear((Vector2)transform.position, potentialTarget))
            {
                return potentialTarget;
            }
        }

        // No valid path found; return the current position to stop moving
        return (Vector2)transform.position;
    }

    // Moves the enemy towards a target position
    void MoveTowards(Vector2 targetPosition)
    {
        Vector2 newPosition = Vector2.MoveTowards((Vector2)transform.position, targetPosition, moveSpeed * Time.deltaTime);
        transform.position = newPosition;
    }
}
