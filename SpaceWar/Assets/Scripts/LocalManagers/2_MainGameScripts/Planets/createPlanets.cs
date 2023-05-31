using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibrary.Models;
using System;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class createPlanets : MonoBehaviour
{
    [SerializeField] private List<GameObject> planetsPrefabs;
    [SerializeField] private GameObject dropdownPrefab;
    public HeroMapView heroMapView = new HeroMapView();
    public float ConnectionsThickness;
   
    void Start()
    {
        #region HeroMapCreation
        heroMapView.HeroId = Guid.NewGuid();
        var planets = new List<Planet>();
        var connections = new List<Edge>();
        for (int i = 0; i < 20; i++)
        {
            Planet planet = new Planet();
            planet.X = UnityEngine.Random.Range(-1000, 1000);
            planet.Y = UnityEngine.Random.Range(-1000, 1000);
            planet.Id = Guid.NewGuid();
            planets.Add(planet);
            connections.Add(new Edge(planets[UnityEngine.Random.Range(0, i)], planets[UnityEngine.Random.Range(0, i)]));
        }
        heroMapView.Planets = planets;
        heroMapView.Connections = connections;
        #endregion

        foreach (var planet in heroMapView.Planets) {
        var index = UnityEngine.Random.Range(0, planetsPrefabs.Count);
            GameObject newPlanet = Instantiate(planetsPrefabs[index]);
           var comp = newPlanet.AddComponent<PlanetController>();
            comp.dropdownPrefab = dropdownPrefab;
            comp.planetPrefab = planetsPrefabs[index];
            newPlanet.transform.SetParent(transform);
            float randomX = planet.X;
            float randomY = planet.Y;
            Vector3 randomPosition = new Vector3(randomX, randomY, 0);
            newPlanet.transform.position = randomPosition;
            // TODO: use planet name instead of id
            newPlanet.name = planet.Id.ToString();
            newPlanet.SetActive(true);
        }
        var connectionsObject = GameObject.Find("Connections");
        foreach (var connection in heroMapView.Connections)
        {
            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.transform.SetParent(connectionsObject.transform);
            cylinder.name = connection.Id.ToString();
            transformCylinder script = cylinder.AddComponent<transformCylinder>();
            script.cylinder = cylinder;
            // TODO: search by name instestead of id
            script.fromPlanet = GameObject.Find(connection.From.Id.ToString()); ;
            script.toPlanet = GameObject.Find(connection.To.Id.ToString()); ;
            script.scale = ConnectionsThickness;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
