using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibrary.Models;
using System;

public class createPlanets : MonoBehaviour
{
    public List<GameObject> planetsPrefabs;
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
            newPlanet.AddComponent<PlanetRotation>();
            newPlanet.transform.SetParent(transform);
            float randomX = planet.X;
            float randomY = planet.Y;
            Vector3 randomPosition = new Vector3(randomX, randomY, 0);
            newPlanet.transform.position = randomPosition;
            // TODO: use planet name instead of id
            newPlanet.name = planet.Id.ToString();
            newPlanet.SetActive(true);
        }
        var res = GameObject.Find("Connections");
        createConnections cr = res.AddComponent<createConnections>();
        cr.connections = heroMapView.Connections;
        cr.scale = ConnectionsThickness;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
