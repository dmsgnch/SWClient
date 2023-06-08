using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
using Color = UnityEngine.Color;

public class ConnectionController : MonoBehaviour
{
    public GameObject fromPlanet;
    public GameObject toPlanet;
    public float thickness;
    
    public void Paint(Color color)
    {
        Material material = new Material(Shader.Find("Standard"));
		material.color = color;

		GetComponent<MeshRenderer>().material = material;
    }

    private void Update()
    {
        Vector3 direction = toPlanet.transform.position - fromPlanet.transform.position;
        float distance = direction.magnitude;
        gameObject.transform.localScale = new Vector3(thickness, distance / 2, thickness);
        gameObject.transform.position = (fromPlanet.transform.position + toPlanet.transform.position) / 2;
        gameObject.transform.up = direction.normalized;
    }
}