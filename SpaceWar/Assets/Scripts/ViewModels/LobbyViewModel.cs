using Assets.Scripts.Components.Abstract;
using LocalManagers.ConnectToGame;
using Assets.Scripts.Managers;
using LocalManagers.RegisterLoginRequests;
using SharedLibrary.Models;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ViewModels.Abstract;
using Microsoft.AspNetCore.SignalR.Client;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Contracts.Hubs;
using System.Threading.Tasks;
using Assets.Scripts.View;

namespace Assets.Scripts.ViewModels
{
    public class LobbyViewModel : ViewModelBase
    {
        public LobbyViewModel()
        { }

        public async Task ExitFromLobby()
        {
            HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
            Guid lobbyId = GameManager.Instance.LobbyDataStore.LobbyId;

            await hubConnection.InvokeAsync(ServerHandlers.Lobby.ExitFromLobby, lobbyId);

            GameManager.Instance.ChangeState(GameState.ConnectToGame);
        }

        /// <summary>
        /// generates sample data for testing lobby functionality
        /// </summary>
        /// <returns>lobby filled with sample data</returns>
        public Lobby GetSampleData()
        {
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var lobby = new Lobby
            {
                LobbyName = "default lobby",
                LobbyInfos = new LobbyInfo[]
                {
                    new LobbyInfo
                    {
                        User = new ApplicationUser
                        {
                            Username = "host",
                            Id = userId1,
                        },
                        LobbyLeader = true,
                        UserId = userId1,
                    },
                    new LobbyInfo
                    {
                        User = new ApplicationUser
                        {
                            Username = "not a host",
                            Id = GameManager.Instance.MainDataStore.UserId,
                        },
                        LobbyLeader = false,
                        UserId = GameManager.Instance.MainDataStore.UserId,
                    },
                },
            };
            GameManager.Instance.LobbyDataStore.IsLobbyLeader = lobby.LobbyInfos.Last().LobbyLeader;
            return lobby;
        }

        /// <summary>
        /// defines whether lobby view should have start button or ready button 
        /// depending on whether current user is lobby leader
        /// </summary>
        /// <param name="startButton">
        /// start button, which should be viewed when current user is a lobby leader
        /// </param>
        /// <param name="readyButton">
        /// ready button, which should be viewed when current user is not a lobby leade
        /// r</param>
        public void DefineButton(LobbyView lobbyView, GameObject startButton, GameObject readyButton)
        {
            if (GameManager.Instance.LobbyDataStore.IsLobbyLeader)
            {
                startButton.SetActive(true);
                readyButton.SetActive(false);
                startButton.GetComponent<Button>().onClick.AddListener(OnStartButtonClick);
				startButton.GetComponent<Button>().onClick.AddListener(lobbyView.PlayButtonClickSound);
			}
            else
            {
                startButton.SetActive(false);
                readyButton.SetActive(true);
                readyButton.GetComponent<Button>().onClick.AddListener(OnReadyButtonClick);
				startButton.GetComponent<Button>().onClick.AddListener(lobbyView.PlayButtonClickSound);
			}
        }

        public void SetInteractableStartButton(GameObject startButton)
        {
            bool isAllReady = PlayersListController.Instance.IsAllPlayersReady();

            startButton.GetComponent<Button>().interactable = isAllReady;
        }

        private async void OnStartButtonClick()
        {
            //TODO: Check ready status

            HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
            Guid lobbyId = GameManager.Instance.LobbyDataStore.LobbyId;

            await hubConnection.InvokeAsync(ServerHandlers.Lobby.CreateSession, new Lobby
            {
                Id = lobbyId
            });
        }

        private async void OnReadyButtonClick()
        {
            HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
            Guid lobbyId = GameManager.Instance.LobbyDataStore.LobbyId;
            await hubConnection.InvokeAsync(ServerHandlers.Lobby.ChangeReadyStatus, lobbyId);
        }
    }
}
