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

        private void ClearChildren(GameObject parent)
        {
            foreach (GameObject child in parent.transform)
            {
                UnityEngine.Object.Destroy(child);
            }
        }
    }
}
