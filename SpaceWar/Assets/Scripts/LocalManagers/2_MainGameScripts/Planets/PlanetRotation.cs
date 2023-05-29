using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;  // Скорость вращения планеты

    void Update()
    {
        // Вращаем планету вокруг своей оси
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
