using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    public float edgeThreshold = 25f;
    public float moveSpeed = 5f;
    public float minX = -1000f;
    public float maxX = 1000f;
    public float minY = -1000f;
    public float maxY = 1000f;

    private void Update()
    {
            Vector3 mousePosition = Input.mousePosition;

            if (transform.position.x > minX && mousePosition.x < edgeThreshold && !IsCursorOverUI())
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            }
            else if (transform.position.x < maxX && mousePosition.x > Screen.width - edgeThreshold && !IsCursorOverUI())
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }

            if (transform.position.y > minY && mousePosition.y < edgeThreshold && !IsCursorOverUI())
            {
                transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            }
            else if (transform.position.y < maxY && mousePosition.y > Screen.height - edgeThreshold - 50 && !IsCursorOverUI())
            {
                transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            }
    }

    private bool IsCursorOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
