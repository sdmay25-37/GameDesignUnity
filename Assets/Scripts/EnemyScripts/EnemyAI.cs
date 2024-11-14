using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public List<LightArea> lightAreas;
    public float moveSpeed = 0.1f;

    private Pathfinding pathfinding;
    private TargetCalculator targetCalculator;
    private List<Vector2> path;
    private int currentPathIndex;

    public float attackRadius = 5.0f;
    private Vector2 roamingTarget;
    private float roamTimer = 0f;      // Timer to trigger a new roaming target
    private float roamInterval = 3f;   // Time interval between picking new roaming targets


    void Start()
    {
        pathfinding = new Pathfinding();
        targetCalculator = new TargetCalculator();
        path = new List<Vector2>();
        currentPathIndex = 0;
    }

    void Update()
    {
        Vector2 playerPosition = player.position;
        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

        // Check if the player is within attack range
        if (distanceToPlayer <= attackRadius)
        {
            // Chase the player
            Vector2 safeTarget = targetCalculator.GetSafeTarget(playerPosition, lightAreas);
            List<PathNode> rawPath = pathfinding.FindPath(transform.position, safeTarget, lightAreas);

            if (rawPath == null || rawPath.Count == 0)
            {
                // Debug.LogWarning("No path found to target.");
                return;
            }

            path = pathfinding.SmoothPath(rawPath);
            currentPathIndex = 0;

            if (path.Count > 0)
            {
                MoveAlongPath();
            }
        }
        else
        {
            // Roam if the player is out of range
            Roam();
        }
    }


    void MoveAlongPath()
    {
        if (path != null && path.Count > 0 && currentPathIndex < path.Count)
        {
            Vector2 targetPosition = path[currentPathIndex];
           // Debug.Log("Moving towards waypoint: " + currentPathIndex + " at position: " + targetPosition);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f) 
            {
                currentPathIndex++;
               // Debug.Log("Reached waypoint " + (currentPathIndex - 1) + ", moving to next.");
            }
        }
        else
        {
           // Debug.Log("Path complete or no valid path available.");
            path = null;
            currentPathIndex = 0;
        }
    }

    void Roam()
    {
        roamTimer -= Time.deltaTime;

        // Pick a new roaming target if the timer runs out or no target is set
        if (roamTimer <= 0 || (roamingTarget != Vector2.zero && Vector2.Distance(transform.position, roamingTarget) < 0.5f))
        {
            roamingTarget = GetRandomRoamingTarget();
            roamTimer = roamInterval;
        }

        // Path to the roaming target and move along it
        List<PathNode> rawPath = pathfinding.FindPath(transform.position, roamingTarget, lightAreas);

        if (rawPath == null || rawPath.Count == 0)
        {
           // Debug.LogWarning("No path found for roaming.");
            return;
        }

        path = pathfinding.SmoothPath(rawPath);
        currentPathIndex = 0;

        if (path.Count > 0)
        {
            MoveAlongPath();
        }
    }

    Vector2 GetRandomRoamingTarget()
    {
        Vector2 randomPosition;
        bool isPositionValid = false;

        // Loop until we find a position outside all light areas
        do
        {
            float randomX = Random.Range(-10f, 10f); // Adjust range based on roaming area
            float randomY = Random.Range(-10f, 10f);
            randomPosition = new Vector2(randomX, randomY);

            // Check if the random position is in any light area
            isPositionValid = true;
            foreach (var light in lightAreas)
            {
                if (light.IsPointInLight(randomPosition))
                {
                    isPositionValid = false;
                    break;
                }
            }
        } while (!isPositionValid);

        return randomPosition;
    }


}
