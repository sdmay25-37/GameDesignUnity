using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LightArea : MonoBehaviour
{
    public float radius = 2f;
    private const float lightEpsilon = 0.1f; // Epsilon to allow a bit of tolerance

    public bool IsPointInLight(Vector2 point)
    {
        float distanceToCenter = Vector2.Distance(transform.position, point);
        bool inLight = distanceToCenter < (radius - lightEpsilon);
        // Debug.Log("Point " + point + " is " + (inLight ? "inside" : "outside") + " light area with center " + transform.position);
        return inLight;
    }

    // Draws the light radius as a wireframe circle in the Scene view
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Color transparentYellow = new Color(1f, 1f, 0f, 0.1f);

        Handles.color = transparentYellow;

        Handles.DrawSolidDisc(transform.position, Vector3.forward, radius);

        Handles.color = Color.yellow; // Solid yellow outline
        Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
#endif
    }
}
