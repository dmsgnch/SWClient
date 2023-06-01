using Assets.Scripts.Managers;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ViewModels.Abstract;

namespace Assets.Scripts.ViewModels
{
    public class PlanetsViewModel : ViewModelBase
    {
        private const float ConnectionThickness = 6f;

        public GameObject[] GeneratePlanets(GameObject[] planetPrefabs,GameObject planetsParent,
            GameObject dropdownPrefab)
        {
            HeroMapView heroMapView = GameManager.Instance.HeroDataStore.HeroMapView;

            List<GameObject> planets = new List<GameObject>();
            foreach (var planet in heroMapView.Planets)
            {
                var index = UnityEngine.Random.Range(0, planetPrefabs.Length);
                GameObject newPlanet = MonoBehaviour.Instantiate(planetPrefabs[index]);
                var planetController = newPlanet.AddComponent<PlanetController>();
                planetController.dropdownPrefab = dropdownPrefab;
                planetController.planetPrefab = planetPrefabs[index];
                newPlanet.transform.SetParent(planetsParent.transform);
                float randomX = planet.X;
                float randomY = planet.Y;
                Vector3 randomPosition = new Vector3(randomX, randomY, 0);
                newPlanet.transform.position = randomPosition;
                // TODO: use planet name instead of id
                newPlanet.name = planet.Id.ToString();
                newPlanet.SetActive(true);
                planets.Add(newPlanet);
            }

            return planets.ToArray();
        }

        public void CreateConnections(GameObject connectionsParent, GameObject[] planets)
        {
            HeroMapView heroMapView = GameManager.Instance.HeroDataStore.HeroMapView;

            foreach (var connection in heroMapView.Connections)
            {
                var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                cylinder.transform.SetParent(connectionsParent.transform);
                cylinder.name = connection.Id.ToString();
                var connectionController = cylinder.AddComponent<ConnectionController>();
                // TODO: search by name instestead of id
                GameObject fromPlanet = planets.FirstOrDefault(
                    p => p.name.Equals(connection.From.Id.ToString()));
                GameObject toPlanet = planets.FirstOrDefault(
                    p => p.name.Equals(connection.To.Id.ToString()));
                
                if(fromPlanet is null || toPlanet is null)
                {
                    throw new ArgumentException("connection contains planet that does not exist!");
                }

                connectionController.fromPlanet = fromPlanet;
                connectionController.toPlanet = toPlanet;
                connectionController.thickness = ConnectionThickness;
            }
        }
    }
}
