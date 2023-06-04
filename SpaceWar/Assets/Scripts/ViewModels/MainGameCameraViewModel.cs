using Assets.Scripts.Managers;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
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

		public void CenterCameraOnCapital(GameObject camera)
        {
            Planet[] planets = GameManager.Instance.HeroDataStore.HeroMapView.Planets.ToArray();
            Guid homePlanetId = GameManager.Instance.HeroDataStore.CapitalPlanetId;

            Planet homePlanet = planets.FirstOrDefault(p => p.Id.Equals(homePlanetId));

            camera.transform.position = new Vector3(homePlanet.X, homePlanet.Y, 
				Mathf.Clamp(camera.transform.position.z, minDistance, maxDistance));
        }

		public void OnBorderMove(GameObject camera, ref Vector3 mousePosition)
		{
			Vector3 cameraMovement = Vector3.zero;
			Vector3 cameraPosition = camera.transform.position;
			if (camera.transform.position.x > minX && mousePosition.x < edgeThreshold && !IsCursorOverUI())
			{
				var moveLeft = Vector3.left * moveSpeed * Time.deltaTime;
				if(IsInLeftBorder(camera.transform.position + cameraMovement + moveLeft))
				   cameraMovement += moveLeft;
            }
			else if (camera.transform.position.x < maxX && 
				mousePosition.x > Screen.width - edgeThreshold && !IsCursorOverUI())
            {
                var moveRight = Vector3.right * moveSpeed * Time.deltaTime;
                if (IsInRightBorder(camera.transform.position + cameraMovement + moveRight))
                    cameraMovement += moveRight;
			}

			if (camera.transform.position.y > minY && mousePosition.y < edgeThreshold && !IsCursorOverUI())
            {
                var moveDown = Vector3.down * moveSpeed * Time.deltaTime;
                if (IsInLowerBorder(camera.transform.position + cameraMovement + moveDown))
                    cameraMovement += moveDown;
			}
			else if (camera.transform.position.y < maxY && 
				mousePosition.y > Screen.height - edgeThreshold && !IsCursorOverUI())
            {
                var moveUp = Vector3.up * moveSpeed * Time.deltaTime;
                if (IsInUpperBorder(camera.transform.position + cameraMovement + moveUp))
                    cameraMovement += moveUp;
			}

			camera.transform.Translate(cameraMovement);
		}

		public void OnMouseMove(GameObject camera, ref Vector3 previousMousePosition)
        {
            Vector3 currentMousePosition = Input.mousePosition;
			Vector3 mouseDelta = currentMousePosition - previousMousePosition;

			Vector3 cameraMovement = -mouseDelta * dragSpeed;
			cameraMovement.z = 0f;

            Planet[] planets = GameManager.Instance.HeroDataStore.HeroMapView?.Planets.ToArray();
            if (planets is null) return;

            float minX = planets.Min(p => p.X);
            float minY = planets.Min(p => p.Y);
            float maxX = planets.Max(p => p.X);
            float maxY = planets.Max(p => p.Y);

			Vector3 newPosition = camera.transform.position + cameraMovement;
			camera.transform.position = MoveIntoPlanetsBorders(newPosition);

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

		private bool IsInLeftBorder(Vector3 position)
        {
            Planet[] planets = GameManager.Instance.HeroDataStore.HeroMapView?.Planets.ToArray();
            if (planets is null) return true;
            float minX = planets.Min(p => p.X);

            bool isInLeftBorder = position.x > minX;
			return isInLeftBorder;
        }

        private bool IsInRightBorder(Vector3 position)
        {
            Planet[] planets = GameManager.Instance.HeroDataStore.HeroMapView?.Planets.ToArray();
            if (planets is null) return true;
            float maxX = planets.Max(p => p.X);

            bool isInRightBorder = position.x < maxX;
            return isInRightBorder;
        }

        private bool IsInUpperBorder(Vector3 position)
        {
            Planet[] planets = GameManager.Instance.HeroDataStore.HeroMapView?.Planets.ToArray();
            if (planets is null) return true;
            float maxY = planets.Max(p => p.Y);

            bool isInUpperBorder = position.y < maxY;
            return isInUpperBorder;
        }

        private bool IsInLowerBorder(Vector3 position)
        {
            Planet[] planets = GameManager.Instance.HeroDataStore.HeroMapView?.Planets.ToArray();
            if (planets is null) return true;
            float minY = planets.Min(p => p.Y);

            bool isInLowerBorder = position.y < maxY;
            return isInLowerBorder;
        }

   //     private bool IsInPlanetsBorders(Vector3 position)
   //     {
   //         Planet[] planets = GameManager.Instance.HeroDataStore.HeroMapView?.Planets.ToArray();
   //         if (planets is null) return true;

   //         float minX = planets.Min(p => p.X);
   //         float minY = planets.Min(p => p.Y);
   //         float maxX = planets.Max(p => p.X);
   //         float maxY = planets.Max(p => p.Y);

			//bool isInLeftBorder = position.x > minX;
   //         bool isInRightBorder = position.x < maxX;
   //         bool isInUpperBorder = position.y > minY;
   //         bool isInLowerBorder = position.y < maxY;

   //         return isInLeftBorder && isInRightBorder && isInUpperBorder && isInLowerBorder;

   //     }

		private Vector3 MoveIntoPlanetsBorders(Vector3 position)
        {
            Planet[] planets = GameManager.Instance.HeroDataStore.HeroMapView?.Planets.ToArray();
			if (planets is null) return position;

			float minX = planets.Min(p => p.X);
            float minY = planets.Min(p => p.Y);
            float maxX = planets.Max(p => p.X);
            float maxY = planets.Max(p => p.Y);

			return new Vector3(
				Mathf.Clamp(position.x, minX, maxX), 
				Mathf.Clamp(position.y, minY, maxY),
                position.z
                );
        }
	}
}
