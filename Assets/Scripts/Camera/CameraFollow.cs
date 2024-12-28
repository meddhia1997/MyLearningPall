using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Speed at which the camera follows the player
    public Vector2 offset; // Offset from the player's position at which the camera will follow

    void FixedUpdate()
    {
        // Calculate the desired position for the camera to follow the player
        Vector2 desiredPosition = (Vector2)target.position + offset;

        // Smoothly move the camera towards the desired position using Lerp
        Vector2 smoothedPosition = Vector2.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position to the smoothed position
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
