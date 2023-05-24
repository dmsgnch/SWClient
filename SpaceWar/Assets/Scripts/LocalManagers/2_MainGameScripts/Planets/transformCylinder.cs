using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class transformCylinder : MonoBehaviour
{
    public GameObject fromPlanet;
    public GameObject toPlanet;
    public float scale;
    public GameObject cylinder;

    void Start()
    {
    }

    private void Update()
    {
        
        Vector3 direction = toPlanet.transform.position - fromPlanet.transform.position;
        float distance = direction.magnitude;
        cylinder.transform.localScale = new Vector3(scale, distance / 2, scale);
        cylinder.transform.position = (fromPlanet.transform.position + toPlanet.transform.position) / 2;
        cylinder.transform.up = direction.normalized;
    }
}