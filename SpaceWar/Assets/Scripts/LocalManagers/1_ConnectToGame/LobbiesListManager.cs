using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SharedLibrary.Models;
using Button = UnityEngine.UI.Button;
using Scripts.RegisterLoginScripts;

namespace LocalManagers.ConnectToGame
{
    /// <summary>
    /// class that controls the display of lobbies list
    /// </summary>
    public class LobbiesListManager : StaticInstance<LobbiesListManager>
    {
        [SerializeField] private GameObject _lobbiesPanel;
        [SerializeField] private GameObject _lobbyPrefab;

        public void Start()
        {
            DisplaySampleLobbies();
        }

        /// <summary>
        /// testing function that displays sample data on the panel
        /// </summary>
        public void DisplaySampleLobbies()
        {
            var lobbies = new Lobby[]
            {
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 1", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 2", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 3", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 4", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 5", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 6", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 7", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 8", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 9", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 10", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 11", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 12", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 13", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 14", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 15", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 16", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 17", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 18", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 19", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 20", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 21", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 22", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 23", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 24", MaxHeroNumbers = 2 },
                new Lobby{ Id = Guid.NewGuid(), LobbyName = "Lobby 25", MaxHeroNumbers = 2 },
            };
            UpdateLobbiesListDisplay(lobbies);
        }
        
        /// <summary>
        /// displays lobbies on lobby panel using lobby template
        /// </summary>
        /// <param name="lobbies">collection of lobbies that you want to be displayed</param>
        public void UpdateLobbiesListDisplay(IList<Lobby> lobbies)
        {
            foreach (Transform child in _lobbiesPanel.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (var lobby in lobbies)
            {
                var lobbyView = Instantiate(_lobbyPrefab.transform.GetChild(0).gameObject, _lobbiesPanel.transform);
                lobbyView.transform.GetChild(0).GetComponent<Text>().text = lobby.LobbyName;

                var lobbyButton = lobbyView.GetComponent<Button>();
                lobbyButton.onClick.RemoveAllListeners();
                lobbyButton.onClick.AddListener(() => 
                {
                    NetworkingManager.Instance.SelectedLobbyId = lobby.Id.ToString();
                });
            }
        }  
    }
}