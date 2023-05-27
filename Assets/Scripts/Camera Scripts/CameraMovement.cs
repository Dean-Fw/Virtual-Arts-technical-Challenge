using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // The center point of the semi-sphere
    public float radius = 5f; // The radius of the semi-sphere
    public float rotationSpeed = 5f;
    public float verticalSpeed = 5f;
    public float minDistance = 1f; // The minimum distance between camera and target
    public float maxDistance = 100f; // The maximum distance between camera and target
    public float zoomSpeed = 5f;

    private float horizontalAngle = 0f;
    private float verticalAngle = 30f;
    private float distance = 10f; // The initial distance between camera and target

    private void Update()
    {
        // Camera movement controls
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the horizontal rotation angle
        float horizontalRotation = horizontalInput * rotationSpeed * Time.deltaTime;
        horizontalAngle += horizontalRotation;

        // Calculate the vertical rotation angle
        float verticalRotation = verticalInput * verticalSpeed * Time.deltaTime;
        verticalAngle += verticalRotation;
        verticalAngle = Mathf.Clamp(verticalAngle, 0f, 90f);

        // Calculate the new camera position
        Vector3 newPosition = target.position + Quaternion.Euler(verticalAngle, horizontalAngle, 0f) * new Vector3(0f, 0f, -radius);
        transform.position = newPosition;

        // Camera zooming controls
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        distance -= scrollInput * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Calculate the new camera position based on the zoom distance
        Vector3 zoomDirection = transform.position - target.position;
        zoomDirection.Normalize();
        newPosition = target.position + zoomDirection * distance;

        // Update the camera position
        transform.position = newPosition;

        // Rotate the camera to look at the target point
        transform.LookAt(target.position);
    }
}
