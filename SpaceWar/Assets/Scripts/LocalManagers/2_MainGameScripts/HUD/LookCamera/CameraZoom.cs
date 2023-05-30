using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 250.0f;
    public float minDistance = -900.0f;
    public float maxDistance = -200.0f;

    private float currentDistance;

    void Start()
    {
        currentDistance = transform.position.z;
    }

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollInput) > 0.0f)
        {
            float zoomDelta = scrollInput * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance + zoomDelta, minDistance, maxDistance);
            Vector3 newPosition = transform.position;
            newPosition.z = currentDistance;
            transform.position = newPosition;
        }
    }
}
