using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyOpener : MonoBehaviour
{
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject connectToGamePanel;

    public void OpenLobby()
    {
        lobbyPanel.SetActive(true);
        connectToGamePanel.SetActive(false);
    }
}
