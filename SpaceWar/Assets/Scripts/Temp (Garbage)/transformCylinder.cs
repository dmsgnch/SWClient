using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transformCylinder : MonoBehaviour
{
    public GameObject fromPlanet;
    public GameObject toPlanet;
    public float scale;

    void Start()
    {
    }

    private void Update()
    {
        Vector3 direction = toPlanet.transform.position - fromPlanet.transform.position;
        float distance = direction.magnitude;
        transform.localScale = new Vector3(scale, distance / 2, scale);
        transform.position = (fromPlanet.transform.position + toPlanet.transform.position) / 2;
        transform.up = direction.normalized;
    }
}
