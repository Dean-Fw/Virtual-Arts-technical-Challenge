using UnityEngine;

public class ShapeDeletion : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                ShapeObject shapeObject = hit.collider.GetComponent<ShapeObject>();
                if (shapeObject != null)
                {
                    // Destroy the shape object
                    Destroy(shapeObject.gameObject);
                }
            }
        }
    }
}
