using UnityEngine;
using UnityEngine.EventSystems;

public class ShapePlacement : MonoBehaviour
{
    public GameObject[] shapePrefabs; // Assign the shape prefabs in the Inspector
    private GameObject selectedShapePrefab;

    private void Start()
    {
        // No shape is selected by default
        selectedShapePrefab = null;
    }

    public void SelectShape(int index)
    {
        // Ensure the index is within valid range
        if (index >= 0 && index < shapePrefabs.Length)
        {
            selectedShapePrefab = shapePrefabs[index];
        }
        else
        {
            selectedShapePrefab = null; // Invalid index, no shape selected
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // Ensure a shape is selected
            if (selectedShapePrefab != null)
            {
                // Raycast to detect the click position in the scene
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Check if the hit object is a shape
                    ShapeObject shapeObject = hit.collider.GetComponent<ShapeObject>();

                    if (shapeObject != null)
                    {
                        // Place the shape on top of the existing shape
                        PlaceShapeOnTop(shapeObject);
                    }
                    else
                    {
                        // Place the shape on the plane
                        PlaceShapeOnPlane(ray, hit.point);
                    }
                }
            }
        }
    }

    private void PlaceShapeOnTop(ShapeObject shapeObject)
    {
        // Calculate the offset based on the size of the shape prefab
        Vector3 shapeSize = selectedShapePrefab.transform.localScale;
        float yOffset = shapeSize.y;

        // Instantiate the selected shape on top of the existing shape
        Vector3 spawnPosition = shapeObject.transform.position + Vector3.up * yOffset;
        GameObject newShape = Instantiate(selectedShapePrefab, spawnPosition, Quaternion.identity);
        Rigidbody shapeRigidbody = newShape.GetComponent<Rigidbody>();

        if (shapeRigidbody != null)
        {
            shapeRigidbody.isKinematic = false; // Enable physics interactions
        }
    }

    private void PlaceShapeOnPlane(Ray ray, Vector3 intersectionPoint)
    {
        // Calculate the offset based on the size of the shape prefab
        Vector3 shapeSize = selectedShapePrefab.transform.localScale;
        float yOffset = shapeSize.y * 0.5f;

        // Instantiate the selected shape on the plane
        Vector3 spawnPosition = intersectionPoint + Vector3.up * yOffset;
        GameObject newShape = Instantiate(selectedShapePrefab, spawnPosition, Quaternion.identity);
        Rigidbody shapeRigidbody = newShape.GetComponent<Rigidbody>();

        if (shapeRigidbody != null)
        {
            shapeRigidbody.isKinematic = false; // Enable physics interactions
        }
    }
}
