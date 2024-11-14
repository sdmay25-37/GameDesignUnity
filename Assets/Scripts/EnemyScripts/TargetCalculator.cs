using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetCalculator
{
    public Vector2 GetSafeTarget(Vector2 playerPosition, List<LightArea> lightAreas)
    {
        if (!lightAreas.Any(light => light.IsPointInLight(playerPosition)))
            return playerPosition;

        Vector2 safePoint = playerPosition;
        float minDistance = float.MaxValue;

        foreach (var light in lightAreas)
        {
            Vector2 direction = (playerPosition - (Vector2)light.transform.position).normalized;
            Vector2 potentialSafePoint = (Vector2)light.transform.position + direction * light.radius;

            if (!lightAreas.Any(l => l.IsPointInLight(potentialSafePoint)))
            {
                float distance = Vector2.Distance(playerPosition, potentialSafePoint);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    safePoint = potentialSafePoint;
                }
            }
        }

        return safePoint;
    }
}
