using UnityEngine;

public class ShapePlacement : MonoBehaviour
{
    public GameObject[] shapePrefabs; // Assign the shape prefabs in the Inspector
    private GameObject selectedShapePrefab;

    private Plane plane;

    private void Start()
    {
        // Select the first shape as the default
        SelectShape(0);

        // Create a plane using the normal of the first shape prefab
        if (shapePrefabs.Length > 0)
        {
            Vector3 normal = shapePrefabs[0].transform.up;
            Vector3 point = shapePrefabs[0].transform.position;
            plane = new Plane(normal, point);
        }
    }

    public void SelectShape(int index)
    {
        // Ensure the index is within valid range
        if (index >= 0 && index < shapePrefabs.Length)
        {
            selectedShapePrefab = shapePrefabs[index];
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast to detect the click position in the scene
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            if (hits.Length > 0)
            {
                // Sort the raycast hits by distance in ascending order
                System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

                // Find the intersection point with the plane
                float distance;
                if (plane.Raycast(ray, out distance))
                {
                    Vector3 intersectionPoint = ray.GetPoint(distance);

                    // Check for valid placement by ignoring hits that are closer than the intersection point
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].distance > distance)
                        {
                            // Calculate an offset based on the shape's size
                            Vector3 shapeSize = selectedShapePrefab.transform.localScale;
                            float yOffset = shapeSize.y * 0.5f;

                            // Instantiate the selected shape at the intersection point with the offset
                            Vector3 spawnPosition = intersectionPoint + Vector3.up * yOffset;
                            GameObject newShape = Instantiate(selectedShapePrefab, spawnPosition, Quaternion.identity);
                            Rigidbody shapeRigidbody = newShape.GetComponent<Rigidbody>();

                            if (shapeRigidbody != null)
                            {
                                shapeRigidbody.isKinematic = false; // Enable physics interactions
                            }

                            break;
                        }
                    }
                }
            }
        }
    }
}
