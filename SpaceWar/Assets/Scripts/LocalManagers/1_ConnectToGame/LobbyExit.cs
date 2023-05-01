using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LocalManagers.ConnectToGame
{
    public class LobbyExit : MonoBehaviour
    {
        [SerializeField] private GameObject lobbyCanvas;
        [SerializeField] private GameObject connectToGameCanvas;
    
        public void GoToGameConnection()
        {
            lobbyCanvas.SetActive(false);
            connectToGameCanvas.SetActive(true);
        }
    }
}

