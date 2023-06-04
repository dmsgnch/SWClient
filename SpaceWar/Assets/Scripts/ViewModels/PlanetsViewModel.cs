using Assets.Scripts.Managers;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using ViewModels.Abstract;
using Vector3 = UnityEngine.Vector3;
using Object = UnityEngine.Object;
using Microsoft.AspNetCore.SignalR.Client;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Contracts.Hubs;
using SharedLibrary.Requests;
using static SharedLibrary.Routes.ApiRoutes;
using SharedLibrary.Models.Enums;
using Components;

namespace Assets.Scripts.ViewModels
{
	public class PlanetsViewModel : ViewModelBase
	{
		private const float ConnectionThickness = 0.5f;

		public async void UpdatePlanet(Planet newPlanet, GameObject textPrefab) {
			var planet = GameObject.Find(newPlanet.PlanetName);
            if (planet is not null)
            {
                var planetController = planet.GetComponentInChildren<PlanetController>();

				//if (planetController.planet.ResourceCount != planet.ResourceCount) {
                    UpdateResourceTextToPlanet(newPlanet, planet.transform.Find("resourceText"));
				    UpdateSizeTextToPlanet(newPlanet, planet.transform.Find("sizeText"));
				planetController.planet = newPlanet;
                    await ShowPlanetsTextsChangePanel(newPlanet.ResourceCount - planetController.planet.ResourceCount
						, newPlanet.Size - planetController.planet.Size, textPrefab, planet);
                //  }
            }
			else throw new DataException();
		}

		private async Task ShowPlanetsTextsChangePanel(int ResValue, int SizeValue, GameObject textPrefab, GameObject planet) {
            MeshRenderer sphereRenderer = planet.GetComponentInChildren<MeshRenderer>();
            Vector3 size = sphereRenderer.bounds.size;
            float diameter = Mathf.Max(size.x, size.y, size.z);

           var sizeText = GameObject.Instantiate(textPrefab);
            sizeText.transform.SetParent(planet.transform);
            sizeText.transform.position = planet.transform.GetChild(0).transform.position
                + (Vector3.down * diameter / 1.15f) + (Vector3.left * diameter / 1.5f);
			var text = sizeText.gameObject.GetComponent<TextMesh>();
			text.text = SizeValue.ToString();
			text.color = SizeValue>=0?Color.green:Color.red;

            var rightDownText = GameObject.Instantiate(textPrefab);
            rightDownText.transform.SetParent(planet.transform);
            rightDownText.transform.position = planet.transform.GetChild(0).transform.position
               + (Vector3.down * diameter / 1.15f) + (Vector3.right * diameter / 3f);
            text = rightDownText.gameObject.GetComponent<TextMesh>();
            text.text = ResValue.ToString();
            text.color = ResValue >= 0 ? Color.green : Color.red;

            await Task.Delay(10000);

				GameObject.Destroy(sizeText);
                GameObject.Destroy(rightDownText);
		}
        private void UpdateSizeTextToPlanet(Planet planet, Transform sizeText)
        {
            if (sizeText is null) return;
            bool isSizeVisible = planet.Status >= PlanetStatus.Colonized &&
                planet.Status < PlanetStatus.Enemy;

            if (!isSizeVisible)
            {
				sizeText.gameObject.SetActive(false);
                return;
            }
            else sizeText.gameObject.SetActive(true);

            sizeText.gameObject.GetComponent<TextMesh>().text = $"S:{planet.Size}\n(+xxx)";
        }
        private void UpdateResourceTextToPlanet(Planet newPlanet, Transform rightDownText)
        {
			if (rightDownText is null) return;
            bool isResourceVisible = newPlanet.Status >= PlanetStatus.Colonized &&
                newPlanet.Status < PlanetStatus.Enemy;

			if (!isResourceVisible) {
				rightDownText.gameObject.SetActive(false);
				return;	
			}
			else rightDownText.gameObject.SetActive(true);


            if (newPlanet.Status is PlanetStatus.Colonized)
            {
                rightDownText.gameObject.GetComponent<TextMesh>().color =
                    GameManager.Instance.HeroDataStore.Color;
            }

            if (newPlanet.ResourceType is ResourceType.OnlyResources)
            {
                rightDownText.gameObject.GetComponent<TextMesh>().text = $"R:{newPlanet.ResourceCount}\n(+xxx)";
            }
            else if (newPlanet.ResourceType is ResourceType.ResourcesWithColonizationShip)
            {
                rightDownText.gameObject.GetComponent<TextMesh>().text = $"CS:{newPlanet.ResourceCount}/limit";
            }
            else if (newPlanet.ResourceType is ResourceType.ResourcesWithResearchShip)
            {
                rightDownText.gameObject.GetComponent<TextMesh>().text = $"RS:{newPlanet.ResourceCount}/limit";
            }
        }
        public GameObject[] GeneratePlanets(GameObject[] planetPrefabs, GameObject planetsParent,
			GameObject planetInfoPanelPrefab, GameObject[] planetIconsPrefabs, GameObject buttonPrefab,
			GameObject planetTextPrefab, GameObject healthbarPrefab)
		{
			ClearChildren(planetsParent);

			HeroMapView heroMapView = GameManager.Instance.HeroDataStore.HeroMapView;

			List<GameObject> planets = new List<GameObject>();
			foreach (var planet in heroMapView.Planets)
			{
				//Create Object for planet and image
				var planetGO = new GameObject(planet.PlanetName);
				planetGO.transform.SetParent(planetsParent.transform);

				//Create planet
				GameObject newPlanet = CreatePlanet(planet, planetPrefabs, planetGO,
					buttonPrefab, planetInfoPanelPrefab, healthbarPrefab); 
				planets.Add(newPlanet);

                //Create image
                MeshRenderer sphereRenderer = newPlanet.GetComponent<MeshRenderer>();
                Vector3 size = sphereRenderer.bounds.size;
                float diameter = Mathf.Max(size.x, size.y, size.z);

				AddStatusIconToPlanet(planet, newPlanet, planetGO, diameter, planetIconsPrefabs);

				AddFortIconToPlanet(planet, newPlanet,planetGO, diameter, planetIconsPrefabs);

				AddSizeTextToPlanet(planet, newPlanet, planetGO, diameter, planetTextPrefab);

				AddResourceTextToPlanet(planet, newPlanet, planetGO, diameter, planetTextPrefab);
            }

			return planets.ToArray();
		}

		public void CreateConnections(GameObject connectionsParent, GameObject[] planets)
		{
			ClearChildren(connectionsParent);

			HeroMapView heroMapView = GameManager.Instance.HeroDataStore.HeroMapView;

			foreach (var connection in heroMapView.Connections)
			{
				var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
				cylinder.transform.SetParent(connectionsParent.transform);
				cylinder.name = connection.Id.ToString();
				// TODO: search by name instestead of id
				GameObject fromPlanet = planets.FirstOrDefault(
					p => p.name.Equals(connection.FromPlanetId.ToString()));
				GameObject toPlanet = planets.FirstOrDefault(
					p => p.name.Equals(connection.ToPlanetId.ToString()));

				if (fromPlanet is null || toPlanet is null)
				{
					throw new ArgumentException("connection contains planet that does not exist!");
				}

				var connectionController = cylinder.AddComponent<ConnectionController>();
				connectionController.fromPlanet = fromPlanet;
				connectionController.toPlanet = toPlanet;
				connectionController.thickness = ConnectionThickness;
			}
		}

        #region PlanetBuilding
        private GameObject CreatePlanet(Planet planet, GameObject[] planetPrefabs,GameObject planetParent,
			GameObject buttonPrefab, GameObject planetInfoPlanetPrefab, GameObject HealthBarPrefab)
        {
            GameObject prefab = GetPlanetPrefabByPlanetType(planet.PlanetType, planetPrefabs);
            GameObject newPlanet = Object.Instantiate(prefab);

            newPlanet.transform.SetParent(planetParent.transform);
            newPlanet.transform.localScale = GetPlanetScale(planet.Size);

            var planetController = newPlanet.AddComponent<PlanetController>();
            planetController.planet = planet;
            planetController.ButtonPrefab = buttonPrefab;
			planetController.InfoPanelPrefab = planetInfoPlanetPrefab;
			planetController.HealthBarPrefab = HealthBarPrefab;

            var planetPosition = new Vector3(planet.X, planet.Y, 0);
            newPlanet.transform.position = planetPosition;

            newPlanet.name = planet.Id.ToString();
            newPlanet.SetActive(true);

			return newPlanet;
        }

		private void AddStatusIconToPlanet(Planet planet, GameObject newPlanet,
			GameObject planetGO, float diameter,GameObject[] planetIconsPrefabs)
		{
            GameObject iconPrefab = GetIconPrefabByPlanetStatus(planet.Status, planetIconsPrefabs);
            if (iconPrefab is not null)
            {
                GameObject statusIcon = Object.Instantiate(iconPrefab);
                statusIcon.transform.SetParent(planetGO.transform);
                statusIcon.transform.GetComponent<SpriteRenderer>().color =
                    GameManager.Instance.HeroDataStore.Color;
                var text = statusIcon.transform.GetComponentInChildren<TMP_Text>();
                if (text is not null) text.text = planet.IterationsLeftToNextStatus.ToString();
                statusIcon.transform.position = newPlanet.transform.position
                    + (Vector3.up * diameter / 1.1f) + (Vector3.left * diameter / 2f);
                statusIcon.transform.localScale = Vector3.one * diameter / 12;
            }
        }

		private void AddFortIconToPlanet(Planet planet, GameObject newPlanet,
            GameObject planetGO, float diameter, GameObject[] planetIconsPrefabs)
		{

            GameObject fortPrefab = GetFortificationPrefab(planet.FortificationLevel, planetIconsPrefabs);
            if (fortPrefab is not null)
            {
                GameObject fortificationIcon = Object.Instantiate(fortPrefab);
                fortificationIcon.transform.SetParent(planetGO.transform);
                fortificationIcon.transform.GetComponent<SpriteRenderer>().color =
                    GameManager.Instance.HeroDataStore.Color;
                fortificationIcon.transform.position = newPlanet.transform.position
                    + (Vector3.up * diameter / 1.1f) + (Vector3.right * diameter / 2f);
                fortificationIcon.transform.localScale = Vector3.one * diameter / 12;
            }
        }

		private void AddSizeTextToPlanet(Planet planet, GameObject newPlanet,
            GameObject planetGO, float diameter, GameObject planetTextPrefab)
		{
			bool isSizeVisible = planet.Status >= PlanetStatus.Colonized &&
				planet.Status < PlanetStatus.Enemy;

            if (!isSizeVisible) return;

            GameObject sizeText = Object.Instantiate(planetTextPrefab);
            sizeText.GetComponent<TextMesh>().text = $"S:{planet.Size}\n(+xxx)";
            sizeText.transform.SetParent(planetGO.transform);
            sizeText.GetComponent<TextMesh>().color = Color.white;
            sizeText.transform.position = newPlanet.transform.position
                + (Vector3.down * diameter / 2f) + (Vector3.left * diameter / 1.5f);
            sizeText.name = "sizeText";
        }

		private void AddResourceTextToPlanet(Planet planet, GameObject newPlanet,
            GameObject planetGO, float diameter, GameObject planetTextPrefab)
        {
            bool isResourceVisible = planet.Status >= PlanetStatus.Colonized &&
                planet.Status < PlanetStatus.Enemy;

			if (!isResourceVisible) return;

            GameObject rightDownText = Object.Instantiate(planetTextPrefab);
            rightDownText.GetComponent<TextMesh>().text = "";
            rightDownText.transform.SetParent(planetGO.transform);
            rightDownText.GetComponent<TextMesh>().color = Color.white;
            rightDownText.transform.position = newPlanet.transform.position
                + (Vector3.down * diameter / 2f) + (Vector3.right * diameter / 3f);
            rightDownText.name = "resourceText";

            if (planet.Status is PlanetStatus.Colonized)
            {
                rightDownText.GetComponent<TextMesh>().color =
                    GameManager.Instance.HeroDataStore.Color;
            }

            if (planet.ResourceType is ResourceType.OnlyResources)
            {
                rightDownText.GetComponent<TextMesh>().text = $"R:{planet.ResourceCount}\n(+xxx)";
            }
            else if (planet.ResourceType is ResourceType.ResourcesWithColonizationShip)
            {
                rightDownText.GetComponent<TextMesh>().text = $"CS:{planet.ResourceCount}/limit";
            }
            else if (planet.ResourceType is ResourceType.ResourcesWithResearchShip)
            {
                rightDownText.GetComponent<TextMesh>().text = $"RS:{planet.ResourceCount}/limit";
            }
        }

        #endregion

        #region SignalR
        public async Task Attack(Planet planet)
		{
			//TODO: Sending signalR request
		}

		public async Task Defend(Planet planet)
		{
			//TODO: Sending signalR request
		}

		public async Task Research(Planet planet)
		{
			await ResearchOrColonizeRequest(planet);
		}

		public async Task Colonize(Planet planet)
		{
			await ResearchOrColonizeRequest(planet);
		}

		private async Task ResearchOrColonizeRequest(Planet planet)
		{
			HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
			Guid lobbyId = GameManager.Instance.LobbyDataStore.LobbyId;

			var request = new UpdatePlanetStatusRequest
			{
				HeroId = GameManager.Instance.HeroDataStore.HeroId,
				SessionId = GameManager.Instance.SessionDataStore.SessionId,
				PlanetId = planet.Id
			};

			await hubConnection.InvokeAsync(ServerHandlers.Session.PostResearchOrColonizePlanet, request);

			GameManager.Instance.ChangeState(GameState.ConnectToGame);
		}

		public async Task BuiltLightDefence(Planet planet)
		{
			//TODO: Sending signalR request
		}

		public async Task BuiltMidleDefence(Planet planet)
		{
			//TODO: Sending signalR request
		}

		public async Task BuiltStrongDefence(Planet planet)
		{
			//TODO: Sending signalR request
		}
        #endregion

        #region ParsingPrefabs

        private GameObject GetPlanetPrefabByPlanetType(PlanetType planetType, GameObject[] prefabs)
		{
			switch (planetType)
			{
				case PlanetType.Mars:
					return prefabs.First(p => p.name.Equals("Mars"));
				case PlanetType.Moon:
					return prefabs.First(p => p.name.Equals("Moon"));
				case PlanetType.Mercury:
					return prefabs.First(p => p.name.Equals("Mercury"));
				case PlanetType.Jupiter:
					return prefabs.First(p => p.name.Equals("Jupiter"));
				case PlanetType.Sun:
					return prefabs.First(p => p.name.Equals("Sun"));
				case PlanetType.Earth:
					return prefabs.First(p => p.name.Equals("Earth"));
				case PlanetType.Venus:
					return prefabs.First(p => p.name.Equals("Venus"));
				default:
					throw new DataException("planet prefab not found");
			}
		}

		private GameObject GetIconPrefabByPlanetStatus(PlanetStatus planetStatus, 
			GameObject[] planetsIconsPrefabs)
		{
			switch (planetStatus)
			{
				case PlanetStatus.Researching:
					return planetsIconsPrefabs.First(p => p.name.Equals("ResearchingIcon"));
                case PlanetStatus.Researched:
                    return planetsIconsPrefabs.First(p => p.name.Equals("ResearchedIcon"));
                case PlanetStatus.HasStation:
                    return planetsIconsPrefabs.First(p => p.name.Equals("HasStationIcon"));
                case PlanetStatus.Colonizing:
                    return planetsIconsPrefabs.First(p => p.name.Equals("ColonizingIcon"));
                case PlanetStatus.Colonized:
                    return planetsIconsPrefabs.First(p => p.name.Equals("ColonizedIcon"));
				case PlanetStatus.Enemy:
					return planetsIconsPrefabs.First(p => p.name.Equals("ColonizedIcon"));
				case PlanetStatus.Known:
					return null;
                default:
                    throw new DataException("status prefab not found");
            }
		}

		private GameObject GetFortificationPrefab(Fortification fortStatus, GameObject[] fortPrefabs)
        {
            switch (fortStatus)
            {
                case Fortification.None:
                    //return fortPrefabs.First(p => p.name.Equals("LightDefenceIcon"));
                    return null;
                case Fortification.Weak:
                    return fortPrefabs.First(p => p.name.Equals("LightDefenceIcon"));
                case Fortification.Reliable:
                    return fortPrefabs.First(p => p.name.Equals("MediumDefenceIcon"));
                case Fortification.Strong:
                    return fortPrefabs.First(p => p.name.Equals("TotalDefenceIcon"));
                default:
                    throw new DataException("Fortification prefab not found");
            }
        }

		#endregion

		private Planet GetPlanetById(Guid id)
		{
			List<Planet> planets = GameManager.Instance.HeroDataStore.HeroMapView.Planets;
			return planets.FirstOrDefault(p => p.Id.Equals(id)) ??
				throw new ArgumentException($"planet with id {id} was not found");
		}
		/// <summary>
		/// clearing children of parent object
		/// </summary>
		/// <param name="parent"></param>
		private void ClearChildren(GameObject parent)
		{
			parent.transform.DestroyChildren();
		}

		private Vector3 GetPlanetScale(int size)
		{
			size = Math.Clamp(size, 1, 25);
			float scale;
			if (size >= 1 && size <= 5)
			{
				scale = 15f;
			}
			else if (size >= 6 && size <= 10)
			{
				scale = 20f;
			}
			else if (size >= 11 && size <= 15)
			{
				scale = 25f;
			}
			else if (size >= 16 && size <= 20)
			{
				scale = 30f;
			}
			else
			{
				scale = 35f;
			}
			scale /= 1.5f;

			return new Vector3(scale, scale, scale);
		}
	}
}
