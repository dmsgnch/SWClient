using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeExitListener : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			//TODO: Add confirm window			 

			if (Debug.isDebugBuild)
			{
				Debug.Log("Application quiting");
			}
			else
			{
				Application.Quit(); //Not work in Unity
			}
		}
	}
}
