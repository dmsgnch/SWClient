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
using Components.Abstract;
using Components;
using SharedLibrary.Responses.Abstract;
using SharedLibrary.Responses;
using Microsoft.AspNetCore.SignalR.Client;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Contracts.Hubs;
using Assets.Scripts.Components;

namespace Assets.Scripts.ViewModels
{
	public class ConnectToGameViewModel : ViewModelBase
	{
		#region Requests

		#region Rest requests

		public static GameObject GetLobbiesListRequestObject { get; set; }
		public static GameObject CreateLobbyRequestSenderObject { get; set; }

		public void SendUpdateLobbiesListRequest()
		{
			GetLobbiesListRequestObject = new GameObject("GetLobbiesRequest");

			var getLobbiesListRequest = GetLobbiesListRequestObject.AddComponent<GetLobbiesListRequestSender>();

			getLobbiesListRequest.GetLobbyList();
		}

		public void SendCreateLobbyRequest()
		{
			CreateLobbyRequestSenderObject = new GameObject("CreateLobbyRequest");

			var createLobbyRequestSender = CreateLobbyRequestSenderObject.AddComponent<CreateLobbyRequestSender>();

			createLobbyRequestSender.CreateLobby();
		}

		#endregion

		#region SignalR 

		public async Task SendConnectToLobbyRequest()
		{
			await NetworkingManager.Instance.StartHub("lobby");

			HubConnection connection = NetworkingManager.Instance.HubConnection;
			Guid lobbyId = GameManager.Instance.ConnectToGameDataStore.SelectedLobbyId;
			string heroName = GameManager.Instance.HeroDataStore.Name;

			await connection.InvokeAsync(ServerHandlers.Lobby.ConnectToLobby, lobbyId, heroName);
		}

		public async Task StopHub()
		{
			await NetworkingManager.Instance.StopHub();
		}

		#endregion

		#endregion

		#region Ui controllers

		public void SetInteractableToButtons(Button connectToGameButton, Button CreateGameButton)
		{
			CreateGameButton.interactable = false;
			connectToGameButton.interactable = false;

			if (IsValidHeroName is true)
			{
				if (IsValidLobbyName is true)				
					CreateGameButton.interactable = true;				
				
				if(LobbiesListController.Instance.IsSelected is true)				
					connectToGameButton.interactable = true;			
			}
		}

		#endregion

		#region Buttons handlers

		public void ToMainMenu()
		{
			GameManager.Instance.ChangeState(GameState.LoadMainMenuScene);
		}

		#endregion

		#region Input fields values change handlers

		protected Color BaseColor { get; set; } = new Color(205, 205, 205, 255);

		public bool IsValidHeroName{ get; set; } = false;
		public bool IsValidLobbyName{ get; set; } = false;

		public void OnHeroNameValueChanged(Image _icon, Sprite _validSprite, Sprite _errorSprite, string value)
		{
			_icon.color = BaseColor;

			DataValidator dataValidator = new DataValidator();

			if (dataValidator.ValidateString(value, out string message))
			{
				GameManager.Instance.ConnectToGameDataStore.HeroName = value;

				_icon.sprite = _validSprite;

				IsValidHeroName = true;

				if (!string.IsNullOrEmpty(message))
					InformationPanelController.Instance.CreateMessage(
							InformationPanelController.MessageType.ERROR, message);
			}
			else
			{
				IsValidHeroName = false;

				_icon.sprite = _errorSprite;
			}
		}

		public void OnLobbyNameValueChanged(Image _icon, Sprite _validSprite, Sprite _errorSprite, string value)
		{
			_icon.color = BaseColor;

			DataValidator dataValidator = new DataValidator();

			if (dataValidator.ValidateString(value, out string message))
			{
				GameManager.Instance.ConnectToGameDataStore.LobbyName = value;

				_icon.sprite = _validSprite;

				IsValidLobbyName = true;

				if (!string.IsNullOrEmpty(message))
					InformationPanelController.Instance.CreateMessage(
						InformationPanelController.MessageType.ERROR, message);
			}
			else
			{
				IsValidLobbyName = false;

				_icon.sprite = _errorSprite;
			}
		}

		#endregion
	}
}
