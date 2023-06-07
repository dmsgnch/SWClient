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
		#region Requests

		public async Task SendExitFromLobbyRequest()
        {
            HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
            Guid lobbyId = GameManager.Instance.LobbyDataStore.LobbyId;

            await hubConnection.InvokeAsync(ServerHandlers.Lobby.ExitFromLobby, lobbyId);

            GameManager.Instance.ChangeState(GameState.ConnectToGame);
        }

		private async void SendCreateSessionRequest()
		{
			HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
			Guid lobbyId = GameManager.Instance.LobbyDataStore.LobbyId;

			await hubConnection.InvokeAsync(ServerHandlers.Lobby.CreateSession, new Lobby
			{
				Id = lobbyId
			});
		}

		private async void SendChangeReadyStatusRequest()
		{
			HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
			Guid lobbyId = GameManager.Instance.LobbyDataStore.LobbyId;

			await hubConnection.InvokeAsync(ServerHandlers.Lobby.ChangeReadyStatus, lobbyId);
		}

		#endregion

		#region UI controllers

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
                startButton.GetComponent<Button>().onClick.AddListener(SendCreateSessionRequest);
				startButton.GetComponent<Button>().onClick.AddListener(lobbyView.PlayButtonClickSound);
			}
            else
            {
                startButton.SetActive(false);
                readyButton.SetActive(true);
                readyButton.GetComponent<Button>().onClick.AddListener(SendChangeReadyStatusRequest);
				startButton.GetComponent<Button>().onClick.AddListener(lobbyView.PlayButtonClickSound);
			}
        }

        public void SetInteractableStartButton(GameObject startButton)
        {
            bool isAllReady = PlayersListController.Instance.IsAllPlayersReady();

            startButton.GetComponent<Button>().interactable = isAllReady;
        }

		#endregion
	}
}
