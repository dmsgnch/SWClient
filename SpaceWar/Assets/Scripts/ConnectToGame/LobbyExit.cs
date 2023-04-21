using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyExit : MonoBehaviour
{
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject connectToGamePanel;

    public void GoToGameConnection()
    {
        lobbyPanel.SetActive(false);
        connectToGamePanel.SetActive(true);
    }
}
