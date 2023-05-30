using UnityEngine;

public class CameraDragFollow : MonoBehaviour
{
    public float dragSpeed = 10;
    public float maxSpeed = 5000;
    public float minX = 0;
    public float maxX = 600;
    public float minY = 0;
    public float maxY = 800;
    public CameraMove cameraMove;

    private Vector3 dragOrigin;
    private bool isDragging = false;

    void Start()
    {
       
    }
        void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
            cameraMove.enabled = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            cameraMove.enabled = true;
        }

        if (isDragging)
        {
            Vector3 dragDelta = Input.mousePosition - dragOrigin;
            float dragDistance = dragDelta.magnitude;
            float speed = Mathf.Clamp(dragDistance * dragSpeed, 0.0f, maxSpeed);
            Vector3 moveDirection = dragDelta.normalized * speed * Time.deltaTime;
            Vector3 newPosition = transform.position + new Vector3(moveDirection.x, moveDirection.y, 0.0f);
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
            transform.position = newPosition;
        }
    }
}
