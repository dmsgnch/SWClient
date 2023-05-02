using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SharedLibrary.Models;
using Button = UnityEngine.UI.Button;
using Scripts.RegisterLoginScripts;
using System.Linq;

namespace LocalManagers.ConnectToGame
{
    /// <summary>
    /// class that controls the display of lobbies list
    /// </summary>
    public class LobbiesListController : StaticInstance<LobbiesListController>
    {
        [SerializeField] private GameObject _lobbiesListItemPrefab;

        //public void OnEnable()
        //{
        //    DisplaySampleLobbies();
        //}

        public void DisplayLobbyList(IList<Lobby> lobbies)
        {
            if (Debug.isDebugBuild)
            {
                DisplaySampleLobbies();
            }
            else
            {
                UpdateLobbiesListDisplay(lobbies);
            }
        }

        /// <summary>
        /// testing function that displays sample data on the panel
        /// </summary>
        public void DisplaySampleLobbies()
        {
            var lobbies = Enumerable.Range(1, 25).Select(l => 
            new Lobby { Id = Guid.NewGuid(), LobbyName = $"Lobby {l}" })
                .ToList();
            UpdateLobbiesListDisplay(lobbies);
        }
        
        /// <summary>
        /// displays lobbies on lobby panel using lobby template
        /// </summary>
        /// <param name="lobbies">collection of lobbies that you want to be displayed</param>
        public void UpdateLobbiesListDisplay(IList<Lobby> lobbies)
        {
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (var lobby in lobbies)
            {
                var lobbyView = Instantiate(_lobbiesListItemPrefab.transform.GetChild(0).gameObject,
                    gameObject.transform);
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