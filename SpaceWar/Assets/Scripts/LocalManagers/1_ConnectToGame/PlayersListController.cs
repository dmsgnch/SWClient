using Scripts.RegisterLoginScripts;
using SharedLibrary.Models;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LocalManagers.ConnectToGame
{
    /// <summary>
    /// class that controls the display of players list in lobby
    /// </summary>
    public class PlayersListController : BehaviorSingleton<PlayersListController>
    {
        [SerializeField] private GameObject _playerPrefab;

        public void UpdatePlayersList(Lobby lobby)
        {
            foreach(Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }

            var hostId = NetworkingManager.Instance.LobbyInfo.Lobby.LobbyInfos.First(l => l.LobbyLeader).UserId;
            foreach (var lobbyInfo in lobby.LobbyInfos)
            {
                var lobbyView = Instantiate(_playerPrefab.transform.GetChild(0).gameObject, gameObject.transform);

                lobbyView.transform.GetChild(0).GetComponent<Text>().text = lobbyInfo.User.Username;
                lobbyView.transform.GetChild(1).GetComponent<Image>().color = Color.blue;

                var toggle = lobbyView.transform.GetChild(2);
                if (lobbyInfo.UserId == hostId)
                {
                    toggle.gameObject.SetActive(false);
                }
                else
                {
                    toggle.GetComponent<Toggle>().interactable = false;
                }
            }
        }
    }
}