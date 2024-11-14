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
        bool inLight = distanceToCenter < (radius - lightEpsilon); // Adjusted check with epsilon
        // Debug.Log("Point " + point + " is " + (inLight ? "inside" : "outside") + " light area with center " + transform.position);
        return inLight;
    }

    // Draws the light radius as a wireframe circle in the Scene view
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        // Set color with transparency (e.g., yellow with 50% opacity)
        Color transparentYellow = new Color(1f, 1f, 0f, 0.1f); // RGB for yellow, alpha for transparency

        // Save the current color
        Handles.color = transparentYellow;

        // Draw a solid transparent circle
        Handles.DrawSolidDisc(transform.position, Vector3.forward, radius);

        // Optionally, draw an outline around the circle to make it clearer
        Handles.color = Color.yellow; // Solid yellow outline
        Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
#endif
    }
}
