using Assets.Scripts.Managers;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ViewModels.Abstract;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.ViewModels
{
    public class PlanetsViewModel : ViewModelBase
    {
        private const float ConnectionThickness = 0.5f;

        public GameObject[] GeneratePlanets(GameObject[] planetPrefabs,GameObject planetsParent,
            GameObject dropdownPrefab)
        {
            ClearChildren(planetsParent);

            HeroMapView heroMapView = GameManager.Instance.HeroDataStore.HeroMapView;

            List<GameObject> planets = new List<GameObject>();
            foreach (var planet in heroMapView.Planets)
            {
                var index = UnityEngine.Random.Range(0, planetPrefabs.Length);
                GameObject newPlanet = MonoBehaviour.Instantiate(planetPrefabs[index]);
                newPlanet.transform.SetParent(planetsParent.transform);
                newPlanet.transform.localScale = GetPlanetScale(planet.Size);

                var planetController = newPlanet.AddComponent<PlanetController>();
                planetController.dropdownPrefab = dropdownPrefab;

                planetController.planetPrefab = planetPrefabs[index];
                var planetPosition = new Vector3(planet.X, planet.Y, 0);
                newPlanet.transform.position = planetPosition;
                // TODO: use planet name instead of id
                newPlanet.name = planet.Id.ToString();
                newPlanet.SetActive(true);

                planets.Add(newPlanet);
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
                
                if(fromPlanet is null || toPlanet is null)
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
            if(size >= 1 && size <= 5)
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

            return new Vector3(scale,scale,scale);
        }

        private Planet GetPlanetById(Guid id)
        {
            List<Planet> planets = GameManager.Instance.HeroDataStore.HeroMapView.Planets;
            return planets.FirstOrDefault(p => p.Id.Equals(id))??
                throw new ArgumentException($"planet with id {id} was not found");
        }

        private void ClearChildren(GameObject parent)
        {
            parent.transform.DestroyChildren();
        }
    }
}
