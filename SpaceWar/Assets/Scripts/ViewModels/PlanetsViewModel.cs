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

namespace Assets.Scripts.ViewModels
{
	public class PlanetsViewModel : ViewModelBase
	{
		private const float ConnectionThickness = 0.5f;

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
				GameObject newPlanet = CreatePlanet(planet, planetPrefabs, planetGO, buttonPrefab, planetInfoPanelPrefab); 
				planets.Add(newPlanet);

                //Create image
                MeshRenderer sphereRenderer = newPlanet.GetComponent<MeshRenderer>();
                Vector3 size = sphereRenderer.bounds.size;
                float diameter = Mathf.Max(size.x, size.y, size.z);

				AddStatusIconToPlanet(planet, newPlanet, planetGO, diameter, planetIconsPrefabs);

				AddFortIconToPlanet(planet, newPlanet,planetGO, diameter, planetIconsPrefabs);

				AddSizeTextToPlanet(planet, newPlanet, planetGO, diameter, planetTextPrefab);

				AddResourceTextToPlanet(planet, newPlanet, planetGO, diameter, planetTextPrefab);

				AddHealthbarToPlanet(planet, newPlanet, planetGO, diameter, healthbarPrefab);
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
			GameObject buttonPrefab, GameObject planetInfoPlanetPrefab)
        {
            GameObject prefab = GetPlanetPrefabByPlanetType(planet.PlanetType, planetPrefabs);
            GameObject newPlanet = Object.Instantiate(prefab);

            newPlanet.transform.SetParent(planetParent.transform);
            newPlanet.transform.localScale = GetPlanetScale(planet.Size);

            var planetController = newPlanet.AddComponent<PlanetController>();
            planetController.planet = planet;
            planetController.ButtonPrefab = buttonPrefab;
			planetController.InfoPanelPrefab = planetInfoPlanetPrefab;

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
            GameObject sizeText = Object.Instantiate(planetTextPrefab);
            sizeText.GetComponent<TextMesh>().text = $"S:{planet.Size}";
            sizeText.transform.SetParent(planetGO.transform);
            sizeText.GetComponent<TextMesh>().color = Color.white;
            sizeText.transform.position = newPlanet.transform.position
                + (Vector3.down * diameter / 2f) + (Vector3.left * diameter / 1.5f);
			sizeText.name = "sizeText";
        }

		private void AddResourceTextToPlanet(Planet planet, GameObject newPlanet,
            GameObject planetGO, float diameter, GameObject planetTextPrefab)
		{
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

            if (planet.Status is PlanetStatus.Researching)
            {
                rightDownText.GetComponent<TextMesh>().text = $"RS:{planet.IterationsLeftToNextStatus}";
            }
            else if (planet.Status is PlanetStatus.Colonizing)
            {
                rightDownText.GetComponent<TextMesh>().text = $"CS:{planet.IterationsLeftToNextStatus}";
            }
            else if (planet.Status is PlanetStatus.Colonized)
            {
                rightDownText.GetComponent<TextMesh>().text = $"R:{planet.Size}";
            }
        }

		private void AddHealthbarToPlanet(Planet planet, GameObject newPlanet,
            GameObject planetGO, float diameter, GameObject planetHealthbarPrefab)
		{
			if (planet.OwnerId is not null && planet.Status is PlanetStatus.Colonized)
            {
                GameObject healthbar = Object.Instantiate(planetHealthbarPrefab);
                healthbar.transform.SetParent(planetGO.transform);
                healthbar.transform.position = newPlanet.transform.position
                    + (Vector3.up * diameter / 2f);
            }
		}
        #endregion

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

		private Planet GetPlanetById(Guid id)
		{
			List<Planet> planets = GameManager.Instance.HeroDataStore.HeroMapView.Planets;
			return planets.FirstOrDefault(p => p.Id.Equals(id)) ??
				throw new ArgumentException($"planet with id {id} was not found");
		}

		private void ClearChildren(GameObject parent)
		{
			foreach (GameObject child in parent.transform)
			{
				Object.Destroy(child);
			}
		}

		public void Attack(Planet planet)
		{
			Debug.Log("Attack");
		}

		public void Defend(Planet planet)
		{
			Debug.Log("Attack");
		}

		public void Research()
		{
			_planetsViewModel.ExecuteNextAction();
		}

		public void Colonize()
		{
			_planetsViewModel.ExecuteNextAction();
		}

		public void BuiltLightDefence()
		{
			_planetsViewModel.ExecuteNextAction();
		}

		public void BuiltMidleDefence()
		{
			_planetsViewModel.ExecuteNextAction();
		}

		public void BuiltStrongDefence()
		{
			_planetsViewModel.ExecuteNextAction();
		}

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
					throw new DataException();
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
                    throw new DataException();
            }
		}

		private GameObject GetFortificationPrefab(Fortification fortStatus, GameObject[] fortPrefabs)
        {
            switch (fortStatus)
            {
                case Fortification.None:
                    return fortPrefabs.First(p => p.name.Equals("LightDefenceIcon"));
                    //return null;
                case Fortification.Weak:
                    return fortPrefabs.First(p => p.name.Equals("LightDefenceIcon"));
                case Fortification.Reliable:
                    return fortPrefabs.First(p => p.name.Equals("MediumDefenceIcon"));
                case Fortification.Strong:
                    return fortPrefabs.First(p => p.name.Equals("TotalDefenceIcon"));
                default:
                    throw new DataException();
            }
        }

        #endregion
    }
}
