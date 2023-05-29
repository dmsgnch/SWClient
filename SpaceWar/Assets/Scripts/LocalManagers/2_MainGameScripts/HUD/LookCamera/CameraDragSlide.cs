using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraDragSlide : MonoBehaviour
{
    private Vector3 previousMousePosition;
    public float dragSpeed = 2f;
    public float minX = -1000f;
    public float maxX = 1000f;
    public float minY = -1000f;
    public float maxY = 1000f;
    public CameraMove cameraMove;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
        }
        

        if (Input.GetMouseButton(0))
        {
            cameraMove.enabled = false;
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - previousMousePosition;

            Vector3 cameraMovement = -mouseDelta * dragSpeed;
            cameraMovement.z = 0f;

            Vector3 newPosition = transform.position + cameraMovement;
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            transform.position = newPosition;

            previousMousePosition = currentMousePosition;
        }
        else cameraMove.enabled = true;
    }
}
