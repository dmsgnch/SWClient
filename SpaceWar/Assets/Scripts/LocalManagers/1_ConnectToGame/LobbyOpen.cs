using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LocalManagers._1_ConnectToGame
{
    public class LobbyOpen : MonoBehaviour
    {
        [SerializeField] private GameObject lobbyCanvas;
        [SerializeField] private GameObject connectToGameCanvas;

        public void OpenLobby()
        {
            lobbyCanvas.SetActive(true);
            connectToGameCanvas.SetActive(false);
        }
    }
}