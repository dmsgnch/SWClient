using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using SharedLibrary.Models;
using UnityEditor.MemoryProfiler;

public class createConnections : MonoBehaviour
{
    public List<SharedLibrary.Models.Edge> connections;
    public float scale;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var connection in connections) {
            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.transform.SetParent(transform);
            cylinder.name = connection.Id.ToString();
            transformCylinder script = cylinder.AddComponent<transformCylinder>();
            script.cylinder = cylinder;
            // TODO: search by name instestead of id
            script.fromPlanet = GameObject.Find(connection.From.Id.ToString()); ;
            script.toPlanet = GameObject.Find(connection.To.Id.ToString()); ;
            script.scale = scale;
            
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
}
