using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDCursorFollower : MonoBehaviour
{
    public Vector2 offsetPercentage = new Vector2(0f, -0.035f);

    void Update()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        Vector3 cursorPosition = Input.mousePosition;

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 elementSize = rectTransform.rect.size;

        Vector2 offsetPixels = new Vector2(screenWidth * offsetPercentage.x, screenHeight * offsetPercentage.y);

        Vector2 localCursorPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, cursorPosition, null, out localCursorPos);

        Vector3 position = new Vector3(localCursorPos.x + (elementSize.x / 2f) + offsetPixels.x, localCursorPos.y - (elementSize.y / 2f) + offsetPixels.y, 0f);

        transform.localPosition = position;
    }
}
