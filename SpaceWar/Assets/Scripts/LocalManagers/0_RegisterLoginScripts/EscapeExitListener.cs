using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeExitListener : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //TODO: Add confirm window
            Application.Quit();
        }
    }
}
