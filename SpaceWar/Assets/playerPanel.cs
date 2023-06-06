using Assets.Scripts.Components.DataStores;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Components.DataStores.SessionDataStore;
using TMPro;
using UnityEngine.UI;
using SharedLibrary.Models;
using Assets.Scripts.Managers;
using Unity.VisualScripting;

public class playerPanel : MonoBehaviour
{
    [SerializeField] GameObject playerName_Prefab;
    [SerializeField] GameObject playerList;
    //SessionDataStore _sessionDataStore;
    //private Text name;
    
    public void UpdatePlayerList()
    {
        var sessionDataStore = GameManager.Instance.SessionDataStore;
        foreach(var player in sessionDataStore.PanelHeroForms)
        {
            GameObject playerPanel = Instantiate(playerName_Prefab,playerList.transform);
            playerPanel.GetComponent<Text>().text = player.HeroName;
        }
    }
}
