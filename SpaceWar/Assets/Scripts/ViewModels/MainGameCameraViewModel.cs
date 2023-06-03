using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using ViewModels.Abstract;

namespace Assets.Scripts.ViewModels
{
	public class MainGameCameraViewModel : ViewModelBase
	{
		private float edgeThreshold = 1f;
		private float moveSpeed = 1500f;
		private float dragSpeed = 1f;
		private float minX = -25f;
		private float maxX = 750f;
		private float minY = -25f;
		private float maxY = 650f;

		private float zoomSpeed = 250.0f;
		private float minDistance = -450.0f;
		private float maxDistance = -200.0f;

		public void OnBorderMove(GameObject camera, ref Vector3 mousePosition)
		{
			if (camera.transform.position.x > minX && mousePosition.x < edgeThreshold && !IsCursorOverUI())
			{
				camera.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
			}
			else if (camera.transform.position.x < maxX && mousePosition.x > Screen.width - edgeThreshold - 10 && !IsCursorOverUI())
			{
				camera.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
			}

			if (camera.transform.position.y > minY && mousePosition.y < edgeThreshold && !IsCursorOverUI())
			{
				camera.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
			}
			else if (camera.transform.position.y < maxY && mousePosition.y > Screen.height - edgeThreshold - 10 && !IsCursorOverUI())
			{
				camera.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
			}
		}

		public void OnMouseMove(GameObject camera, ref Vector3 previousMousePosition)
		{
			Vector3 currentMousePosition = Input.mousePosition;
			Vector3 mouseDelta = currentMousePosition - previousMousePosition;

			Vector3 cameraMovement = -mouseDelta * dragSpeed;
			cameraMovement.z = 0f;

			Vector3 newPosition = camera.transform.position + cameraMovement;
			newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
			newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

			camera.transform.position = newPosition;

			previousMousePosition = currentMousePosition;
		}

		public void OnZoom(ref float scrollInput, ref float currentDistance, GameObject camera)
		{
			if (Mathf.Abs(scrollInput) > 0.0f)
			{
				float zoomDelta = scrollInput * zoomSpeed;
				currentDistance = Mathf.Clamp(currentDistance + zoomDelta, minDistance, maxDistance);
				Vector3 newPosition = camera.transform.position;
				newPosition.z = currentDistance;
				camera.transform.position = newPosition;
			}
		}

		private bool IsCursorOverUI()
		{
			return EventSystem.current.IsPointerOverGameObject();
		}
	}
}
