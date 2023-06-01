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

namespace Assets.Scripts.ViewModels
{
	public class PlanetsViewModel : ViewModelBase
	{
		private const float ConnectionThickness = 0.5f;

		public GameObject[] GeneratePlanets(GameObject[] planetPrefabs, GameObject planetsParent,
			GameObject dropdownPrefab, GameObject[] planetIconsPrefabs)
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
				GameObject prefab = GetPrefabByPlanetType(planet.PlanetType, planetPrefabs);
				GameObject newPlanet = MonoBehaviour.Instantiate(prefab);

				newPlanet.transform.SetParent(planetGO.transform);
				newPlanet.transform.localScale = GetPlanetScale(planet.Size);

				var planetController = newPlanet.AddComponent<PlanetController>();
				planetController.dropdownPrefab = dropdownPrefab;

				var planetPosition = new Vector3(planet.X, planet.Y, 0);
				newPlanet.transform.position = planetPosition;

				newPlanet.name = planet.Id.ToString();
				newPlanet.SetActive(true);

				planets.Add(newPlanet);

                //Create image

                GameObject iconPrefab = GetPrefabByPlanetStatus((PlanetStatus)planet.Status, planetIconsPrefabs);
				if (iconPrefab is null) continue;	

                GameObject newIcon = MonoBehaviour.Instantiate(iconPrefab);
                newIcon.transform.SetParent(planetGO.transform);
                newIcon.transform.GetComponent<SpriteRenderer>().color = GameManager.Instance.HeroDataStore.Color;
				var text = newIcon.transform.GetComponentInChildren<TMP_Text>();
				if (text is not null) text.text = planet.IterationsLeftToNextStatus.ToString();				

                newIcon.transform.position = newPlanet.transform.position + (Vector3.up * 30);
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
				UnityEngine.Object.Destroy(child);
			}
		}

		public GameObject GetPrefabByPlanetType(PlanetType planetType, GameObject[] prefabs)
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

		private GameObject GetPrefabByPlanetStatus(PlanetStatus planetStatus, GameObject[] planetsIconsPrefabs)
		{
			switch (planetStatus)
			{
				case PlanetStatus.Researching:
					return planetsIconsPrefabs.First(p => p.name.Equals("ResearchingIcon"));
                case PlanetStatus.Researched:
                    return planetsIconsPrefabs.First(p => p.name.Equals("ColonizingIcon"));
                case PlanetStatus.HasStation:
                    return planetsIconsPrefabs.First(p => p.name.Equals("HasStationIcon"));
                case PlanetStatus.Colonizing:
                    return planetsIconsPrefabs.First(p => p.name.Equals("ColonizingIcon"));
                case PlanetStatus.Colonized:
                    return planetsIconsPrefabs.First(p => p.name.Equals("ColonizedIcon"));
                case PlanetStatus.Known:
					return null;
                default:
                    throw new DataException();
            }

		}
	}
}
