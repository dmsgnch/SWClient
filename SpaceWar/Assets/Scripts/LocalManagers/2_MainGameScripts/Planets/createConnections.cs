using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class createConnections : MonoBehaviour
{
    public List<PlanetPair> planetPairs;
    public float scale;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var pair in planetPairs) {
            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.transform.SetParent(transform);
            transformCylinder script = cylinder.AddComponent<transformCylinder>();
            script.cylinder = cylinder;
            script.fromPlanet = pair.from;
            script.toPlanet = pair.to;
            script.scale = scale;
            
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [System.Serializable]
    public class PlanetPair
    {
        public GameObject from;
        public GameObject to;
    }
}
