using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform player; // Reference to the player's transform

    [SerializeField]
    private Vector2 deadZone = new Vector2(2.0f, 1.5f); // Horizontal and vertical dead zone dimensions

    [SerializeField]
    private float smoothSpeed = 5f; // Camera smoothing speed

    private Vector3 targetPosition;

    void Update()
    {
        // Calculate the difference between the player and camera positions
        Vector3 delta = player.position - transform.position;

        // Start with the current camera position
        targetPosition = transform.position;

        // Check if the player has moved outside the horizontal dead zone
        if (Mathf.Abs(delta.x) > deadZone.x)
        {
            targetPosition.x = player.position.x - Mathf.Sign(delta.x) * deadZone.x;
        }

        // Check if the player has moved outside the vertical dead zone
        if (Mathf.Abs(delta.y) > deadZone.y)
        {
            targetPosition.y = player.position.y - Mathf.Sign(delta.y) * deadZone.y;
        }

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
