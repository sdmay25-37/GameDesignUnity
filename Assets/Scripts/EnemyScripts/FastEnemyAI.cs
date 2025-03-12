using System.Collections.Generic;
using UnityEngine;

public class FastEnemyAI : MonoBehaviour
{
    public Transform player;
    public List<LightArea> lightAreas;
    public float moveSpeed = 0.2f;

    private Pathfinding pathfinding;
    private TargetCalculator targetCalculator;
    private List<Vector2> path;
    private int currentPathIndex;

    public float attackRadius = 4.0f;
    private Vector2 roamingTarget;
    private float roamTimer = 0f;      // Timer to trigger a new roaming target
    private float roamInterval = 3f;   // Time interval between picking new roaming targets
    private float roamCooldown = 2f;  // Cooldown before switching back to roam
    private float cooldownTimer = 0f; // Tracks time since last roam switch


    void Start()
    {
        pathfinding = new Pathfinding();
        targetCalculator = new TargetCalculator();
        path = new List<Vector2>();
        currentPathIndex = 0;
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        Vector2 playerPosition = player.position;
        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

        // Check if the player is within attack range
        if (distanceToPlayer <= attackRadius && cooldownTimer <= 0f)
        {
            // Chase the player
            Vector2 safeTarget = targetCalculator.GetSafeTarget(playerPosition, lightAreas);
            List<PathNode> rawPath = pathfinding.FindPath(transform.position, safeTarget, lightAreas);

            if (rawPath == null || rawPath.Count == 0)
            {
                // If no path is found, fallback to roaming
                Debug.LogWarning("Pathfinding failed. Falling back to roam.");
                StartRoam();
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

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentPathIndex++;
            }
        }
        else
        {
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
            Debug.LogWarning("No path found for roaming.");
            return;
        }

        path = pathfinding.SmoothPath(rawPath);
        currentPathIndex = 0;

        if (path.Count > 0)
        {
            MoveAlongPath();
        }
    }

    void StartRoam()
    {
        cooldownTimer = roamCooldown;
        Roam();
    }

    Vector2 GetRandomRoamingTarget()
    {
        Vector2 randomPosition;
        bool isPositionValid = false;

        Vector2 playerPosition2D = new Vector2(player.position.x, player.position.y);

        // Generate a random target near the player within 45 degrees, this is to deal with issues of the enemy not finding a path too often
        float randomAngle = Random.Range(-45f, 45f);
        Vector2 directionToPlayer = (playerPosition2D - (Vector2)transform.position).normalized;
        float distance = Random.Range(3f, 8f); // Random distance for roaming
        randomPosition = (Vector2)transform.position + RotateVector(directionToPlayer, randomAngle) * distance;

        // Validate that the position is outside all light areas
        foreach (var light in lightAreas)
        {
            if (light.IsPointInLight(randomPosition))
            {
                isPositionValid = false;
                break;
            }
            isPositionValid = true;
        }

        return isPositionValid ? randomPosition : (Vector2)transform.position;
    }


    Vector2 RotateVector(Vector2 vector, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(
            cos * vector.x - sin * vector.y,
            sin * vector.x + cos * vector.y
        );
    }
}
