using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;

public class LobbiesListViewer : MonoBehaviour
{
    [SerializeField] private GameObject lobbiesPanel;
    [SerializeField] private GameObject lobbyTemplate;

    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            var element = Instantiate(lobbyTemplate, lobbiesPanel.transform);
            element.transform.GetChild(0).GetComponent<Text>().text += new System.Random().Next() % 5;
            element.SetActive(true);
        }
    }   
}
