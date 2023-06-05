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
using Random = UnityEngine.Random;
using Components;

namespace Assets.Scripts.ViewModels
{
	public class PlanetsViewModel : ViewModelBase
	{
		private const float ConnectionThickness = 0.5f;
		
		public void GeneratePlanets(PlanetsGenerationForm planetGenerationForm)
		{
			ClearChildren(planetGenerationForm.PlanetsParent);

			var planets = GameManager.Instance.HeroDataStore.HeroMapView.Planets;

			foreach (var planet in planets)
			{
				CreatePlanetGameObject(planet, planetGenerationForm);
            }
		}

		public void CreateConnections(GameObject connectionsParent)
		{
			ClearChildren(connectionsParent);

            GameObject[] planets = GetPlanets();

			var connections = GameManager.Instance.HeroDataStore.HeroMapView.Connections;

			foreach (var connection in connections)
			{
                CreateConnection(connection,connectionsParent,planets);
			}
		}

		public void UpdatePlanet(Planet planet, PlanetsGenerationForm planetsGenerationForm)
		{
			//TODO: find planet on scene by name
			GameObject planetObject = GameObject.Find(planet.PlanetName);
			Object.Destroy(planetObject);

			CreatePlanetGameObject(planet,planetsGenerationForm);
		}
        public void UpdateConnection(Edge connection, GameObject connectionsParent)
        {
            GameObject connectionObject = GameObject.Find(connection.Id.ToString());
            Object.Destroy(connectionObject);

            GameObject[] planets = GetPlanets();
            CreateConnection(connection, connectionsParent, planets);
        }

        #region PlanetBuilding
        private GameObject CreatePlanetGameObject(Planet planet, PlanetsGenerationForm planetGenerationForm)
        {
            //Create Object for planet and image
            var planetGO = new GameObject(planet.PlanetName);
            planetGO.transform.SetParent(planetGenerationForm.PlanetsParent.transform);

            //Create planet
            GameObject newPlanet = CreatePlanetSphere(planet, planetGenerationForm);
            newPlanet.transform.SetParent(planetGO.transform);

            //Create image
            MeshRenderer sphereRenderer = newPlanet.GetComponent<MeshRenderer>();
            Vector3 size = sphereRenderer.bounds.size;
            float diameter = Mathf.Max(size.x, size.y, size.z);

            var planetCreationFom = new PlanetCreationForm
            {
                Planet = planet,
                PlanetGenerationForm = planetGenerationForm,
                PlanetGO = planetGO,
                Diameter = diameter,
            };

            AddStatusIconToPlanet(planetCreationFom);

            AddFortIconToPlanet(planetCreationFom);

            AddSizeTextToPlanet(planetCreationFom);

            AddResourceTextToPlanet(planetCreationFom);

            return newPlanet;
        }

        private GameObject CreatePlanetSphere(Planet planet, PlanetsGenerationForm planetGenerationForm)
        {
            GameObject prefab = GetPlanetPrefabByPlanetType(planet.PlanetType,
                planetGenerationForm.PlanetPrefabs);
            GameObject newPlanet = Object.Instantiate(prefab);

            newPlanet.transform.SetParent(planetGenerationForm.PlanetsParent.transform);
            newPlanet.transform.localScale = GetPlanetScale(planet.Size);

            var planetController = newPlanet.AddComponent<PlanetController>();
            planetController.planet = planet;
            planetController.ButtonPrefab = planetGenerationForm.ButtonPrefab;
            planetController.InfoPanelPrefab = planetGenerationForm.PlanetInfoPanelPrefab;
            planetController.HealthBarPrefab = planetGenerationForm.HealthbarPrefab;

            var planetPosition = new Vector3(planet.X, planet.Y, 0);
            newPlanet.transform.position = planetPosition;

            newPlanet.name = planet.Id.ToString();
            newPlanet.SetActive(true);

            return newPlanet;
        }

        private void AddStatusIconToPlanet(PlanetCreationForm planetCreationForm)
        {
            GameObject iconPrefab = GetIconPrefabByPlanetStatus(planetCreationForm.Planet.Status,
                planetCreationForm.PlanetGenerationForm.PlanetIconsPrefabs);
            if (iconPrefab is null) return;

            GameObject statusIcon = Object.Instantiate(iconPrefab);
            statusIcon.transform.SetParent(planetCreationForm.PlanetGO.transform);
            statusIcon.transform.GetComponent<SpriteRenderer>().color =
                GameManager.Instance.HeroDataStore.Color;
            var text = statusIcon.transform.GetComponentInChildren<TMP_Text>();
            if (text is not null) text.text = planetCreationForm.Planet.IterationsLeftToNextStatus.ToString();

            GameObject planetSphere = planetCreationForm.PlanetGO.transform.GetChild(0).gameObject;

            statusIcon.transform.position = planetSphere.transform.position
                + (Vector3.up * planetCreationForm.Diameter / 1.1f)
                + (Vector3.left * planetCreationForm.Diameter / 2f);
            statusIcon.transform.localScale = Vector3.one * planetCreationForm.Diameter / 12;
        }

        private void AddFortIconToPlanet(PlanetCreationForm planetCreationForm)
        {

            GameObject fortPrefab = GetFortificationPrefab(planetCreationForm.Planet.FortificationLevel,
                planetCreationForm.PlanetGenerationForm.PlanetIconsPrefabs);
            if (fortPrefab is not null)
            {
                GameObject fortificationIcon = Object.Instantiate(fortPrefab);
                fortificationIcon.transform.SetParent(planetCreationForm.PlanetGO.transform);
                fortificationIcon.transform.GetComponent<SpriteRenderer>().color =
                    GameManager.Instance.HeroDataStore.Color;

                GameObject planetSphere = planetCreationForm.PlanetGO.transform.GetChild(0).gameObject;

                fortificationIcon.transform.position = planetSphere.transform.position
                    + (Vector3.up * planetCreationForm.Diameter / 1.1f)
                    + (Vector3.right * planetCreationForm.Diameter / 2f);
                fortificationIcon.transform.localScale = Vector3.one * planetCreationForm.Diameter / 12;
            }
        }

        private void AddSizeTextToPlanet(PlanetCreationForm planetCreationForm)
        {
            bool isSizeVisible = planetCreationForm.Planet.Status >= PlanetStatus.Colonized &&
                planetCreationForm.Planet.Status < PlanetStatus.Enemy;

            if (!isSizeVisible) return;

            GameObject sizeText = Object.Instantiate(planetCreationForm.PlanetGenerationForm.PlanetTextPrefab);
            sizeText.GetComponent<TextMesh>().text = $"S:{planetCreationForm.Planet.Size}";
            sizeText.transform.SetParent(planetCreationForm.PlanetGO.transform);
            sizeText.GetComponent<TextMesh>().color = Color.white;

            GameObject planetSphere = planetCreationForm.PlanetGO.transform.GetChild(0).gameObject;

            sizeText.transform.position = planetSphere.transform.position
                + (Vector3.down * planetCreationForm.Diameter / 2f)
                + (Vector3.left * planetCreationForm.Diameter / 1.5f);
            sizeText.name = "sizeText";
        }

        private void AddResourceTextToPlanet(PlanetCreationForm planetCreationForm)
        {
            bool isResourceVisible = planetCreationForm.Planet.Status >= PlanetStatus.Colonized &&
                planetCreationForm.Planet.Status < PlanetStatus.Enemy;
            Planet planet = planetCreationForm.Planet;

            if (!isResourceVisible) return;

            GameObject rightDownText = Object.Instantiate(
                planetCreationForm.PlanetGenerationForm.PlanetTextPrefab);
            rightDownText.GetComponent<TextMesh>().text = "";
            rightDownText.transform.SetParent(planetCreationForm.PlanetGO.transform);
            rightDownText.GetComponent<TextMesh>().color = Color.white;

            GameObject planetSphere = planetCreationForm.PlanetGO.transform.GetChild(0).gameObject;

            rightDownText.transform.position = planetSphere.transform.position
                + (Vector3.down * planetCreationForm.Diameter / 2f)
                + (Vector3.right * planetCreationForm.Diameter / 3f);
            rightDownText.name = "resourceText";

            if (planet.Status is PlanetStatus.Colonized)
            {
                rightDownText.GetComponent<TextMesh>().color =
                    GameManager.Instance.HeroDataStore.Color;
            }

            if (planet.ResourceType is ResourceType.OnlyResources)
            {
                rightDownText.GetComponent<TextMesh>().text = $"R:{planet.ResourceCount}";
            }
            else if (planet.ResourceType is ResourceType.ColonizationShip)
            {
                rightDownText.GetComponent<TextMesh>().text = $"CS:{planet.ResourceCount}";
            }
            else if (planet.ResourceType is ResourceType.ResearchShip)
            {
                rightDownText.GetComponent<TextMesh>().text = $"RS:{planet.ResourceCount}";
            }
        }

        private void CreateConnection(Edge connection, GameObject connectionsParent, GameObject[] planets)
        {
            GameObject fromPlanet = planets.FirstOrDefault(
                p => p.name.Equals(connection.FromPlanetId.ToString()));
            GameObject toPlanet = planets.FirstOrDefault(
                p => p.name.Equals(connection.ToPlanetId.ToString()));

            if (fromPlanet is null || toPlanet is null)
            {
                throw new ArgumentException("connection contains planet that does not exist!");
            }

            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.transform.SetParent(connectionsParent.transform);
            cylinder.name = connection.Id.ToString();
            // TODO: search by name instestead of id

            var connectionController = cylinder.AddComponent<ConnectionController>();
            connectionController.fromPlanet = fromPlanet;
            connectionController.toPlanet = toPlanet;
            connectionController.thickness = ConnectionThickness;
        }

        private GameObject[] GetPlanets()
        {
            List<GameObject> planets = new List<GameObject>();
            foreach (Transform planetGO in GameObject.Find("Planets").transform)
            {
                planets.Add(planetGO.GetChild(0).gameObject);
            }
            return planets.ToArray();
        }
        #endregion

        #region SignalR
        public async Task Attack(Planet planetToAttack)
		{
			Edge[] connections = GameManager.Instance.HeroDataStore.HeroMapView.Connections.ToArray();
			Planet[] planets = GameManager.Instance.HeroDataStore.HeroMapView.Planets.ToArray();

            Edge[] connectionsWithAttackedPlanet = connections
				.Where(c => c.FromPlanetId.Equals(planetToAttack.Id)
				|| c.ToPlanetId.Equals(planetToAttack.Id))
				.ToArray();
            Planet[] attackingPlanets = connectionsWithAttackedPlanet
				.Select(c => 
				{
					if (c.FromPlanetId.Equals(planetToAttack.Id))
					{
						return planets.FirstOrDefault(p => p.Id.Equals(c.ToPlanetId));
					}
					else
                    {
                        return planets.FirstOrDefault(p => p.Id.Equals(c.FromPlanetId));
                    }
				}).ToArray();

			Planet attackingPlanet = attackingPlanets[0];

			Guid heroId = GameManager.Instance.HeroDataStore.HeroId;
			int soldiers = GameManager.Instance.HeroDataStore.AvailableSoldiers;
            var startBattleRequest = new StartBattleRequest
			{
				HeroId = heroId,
				FromPlanetId = attackingPlanet.Id,
				AttackedPlanetId = planetToAttack.Id,
				CountOfSoldiers = soldiers
            };

			HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
			await hubConnection.SendAsync(ServerHandlers.Session.StartBattle,startBattleRequest);
		}

		public async Task Defend(Planet planetToDefend)
		{
			//TODO: Sending signalR request
			Guid heroId = GameManager.Instance.HeroDataStore.HeroId;
			int soldiers = GameManager.Instance.HeroDataStore.AvailableSoldiers;

			var defendPlanetRequest = new DefendPlanetRequest 
			{
				HeroId = heroId,
				AttackedPlanetId= planetToDefend.Id,
				CountOfSoldiers = soldiers
			};

			HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
			await hubConnection.InvokeAsync(ServerHandlers.Session.DefendPlanet,defendPlanetRequest);
		}

		public async Task ResearchOrColonizeRequest(Planet planet)
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

		public async Task BuildDefence(Planet planet)
		{
			//TODO: Sending signalR request
			Guid heroid = GameManager.Instance.HeroDataStore.HeroId;
			Guid sessionid = GameManager.Instance.SessionDataStore.SessionId;

			var updatePlanetStatusRequest = new UpdatePlanetStatusRequest
			{
				HeroId = heroid,
				SessionId = sessionid,
				PlanetId = planet.Id
			};

			HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
			await hubConnection.InvokeAsync(ServerHandlers.Session.BuildFortification,
				updatePlanetStatusRequest);
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
			parent.transform.DetachChildren();
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

		private class PlanetCreationForm
		{
			public Planet Planet;
            public PlanetsGenerationForm PlanetGenerationForm;
			public GameObject PlanetGO;
			public float Diameter;
        }
	}
}
