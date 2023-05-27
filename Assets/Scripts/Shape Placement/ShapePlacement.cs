using UnityEngine;
using UnityEngine.EventSystems;

public class ShapePlacement : MonoBehaviour
{
    public GameObject[] shapePrefabs; // Assign the shape prefabs in the Inspector
    public float moveSpeed;

    private GameObject selectedShapePrefab;
    private ShapeObject heldShapeObject;


    private void Start()
    {
        // No shape is selected by default
        selectedShapePrefab = null;
    }


    public void SelectShape(int index)
    {
        // Ensure the index is within valid range
        if (index >= 0 && index < shapePrefabs.Length - 1)
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
            else
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
                        // Highlight shape as selected
                        HighlightShape(shapeObject);
                    }
                }
            }

            
        }
        // Check if a shape is currently held
        if (heldShapeObject != null && Input.GetMouseButton(0))
        {
            // Create a layer mask that excludes the shape collider
            LayerMask layerMask = ~LayerMask.GetMask("Shape");
            // Raycast to detect the position in the scene
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                // Move the held shape to the new position
                Vector3 shapeSize = heldShapeObject.transform.localScale;
                float yOffset = 0f;
                if (heldShapeObject.tag == "Cube")
                {
                    yOffset = shapeSize.y * 0.5f;
                }
                else if (heldShapeObject.tag == "Cylinder")
                {
                    yOffset = shapeSize.y * 1f;
                }
                else if (heldShapeObject.tag == "Capsule")
                {
                    yOffset = shapeSize.y * 1f;
                }


                heldShapeObject.transform.position = new Vector3(hit.point.x, yOffset, hit.point.z);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Reset the held shape
            if (heldShapeObject != null)
            {
                // De-highlight the shape
                DehighlightShape(heldShapeObject);
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

    private void HighlightShape(ShapeObject shapeObject)
    {
        // set the color of the held shape to blue
        Renderer shapeRenderer = shapeObject.GetComponent<Renderer>();
        shapeRenderer.material.color = Color.blue;
        heldShapeObject = shapeObject;
    }

    private void DehighlightShape(ShapeObject shapeObject)
    {
        // Reset the color of the shape back to red
        Renderer shapeRenderer = shapeObject.GetComponent<Renderer>();
        shapeRenderer.material.color = Color.red;
        heldShapeObject = null;
    }
}
