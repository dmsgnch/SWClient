using Assets.Scripts.Components.Abstract;
using Assets.Scripts.Managers;
using LocalManagers.ConnectToGame;
using LocalManagers.ConnectToGame.Requests;
using LocalManagers.RegisterLoginRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using ViewModels.Abstract;
using LocalManagers.ConnectToGame.ValueChangedHandlers;
using Components.Abstract;
using Components;
using SharedLibrary.Responses.Abstract;
using SharedLibrary.Responses;
using Microsoft.AspNetCore.SignalR.Client;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Contracts.Hubs;

namespace Assets.Scripts.ViewModels
{
	public class ConnectToGameViewModel : ViewModelBase
	{
		public static GetLobbiesListRequest GetLobbiesListRequest { get; set; }

		public ConnectToGameViewModel()
		{ }

		public void UpdateLobbiesList(GameObject lobbiesListItemPrefab, Button connectToGameButton)
		{
			GetLobbiesListRequest = new GameObject("GetLobbiesRequest").AddComponent<GetLobbiesListRequest>();

			GetLobbiesListRequest.GetLobbyList();

			LobbiesListController.Instance.UpdateLobbiesListDisplay(
				GameManager.Instance.ConnectToGameDataStore.Lobbies, 
				lobbiesListItemPrefab, 
				connectToGameButton);
		}

		public void ToLobby()
		{
			GameManager.Instance.ChangeState(GameState.Lobby);
		}

		public void SetInteractableToButtons(Button connectToGameButton, Button CreateGameButton)
		{
			CreateGameButton.interactable = false;
			connectToGameButton.interactable = false;

			if (HeroNameChangedHandler.Instance.IsValidated is true)
			{
				if (LobbyNameChangedHandler.Instance.IsValidated is true)				
					CreateGameButton.interactable = true;				
				
				if(LobbiesListController.Instance.IsSelected is true)				
					connectToGameButton.interactable = true;			
			}
		}

		public void CloseApplication()
		{
			if (Debug.isDebugBuild)
				Debug.Log("Application quiting");

			Application.Quit();
		}

		public void ConnectToLobby()
        {
			HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
			Guid lobbyId = GameManager.Instance.ConnectToGameDataStore.SelectedLobbyId;
			hubConnection.InvokeAsync(ServerHandlers.Lobby.ConnectToLobby,lobbyId).Wait();
        }

		public class GetAllLobbiesResponseHandler : IResponseHandler
		{
			public void BodyConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
				where T : ResponseBase
			{
				//Not create information panel
			}

			public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
				where T : ResponseBase
			{
				GameManager.Instance.ConnectToGameDataStore.Lobbies = 
					requestForm.GetResponseResult<GetAllLobbiesResponse>().Lobbies;
			}

			public void OnRequestFinished()
			{
				Destroy(GetLobbiesListRequest.gameObject);
			}
		}
	}
}
