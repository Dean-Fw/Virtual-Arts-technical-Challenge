using UnityEngine;

public class ShapeObject : MonoBehaviour
{
    public GameObject shapePrefab;
    private float offset;

    public void MoveTo(Vector3 position)
    {
        transform.position = position;
    }

    public void Rotate(float scroll)
    {
        transform.Rotate(Vector3.up, scroll * 10f);
    }



}
