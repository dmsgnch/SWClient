using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using SharedLibrary.Responses.Abstract;
using UnityEngine.Networking;
using Components;
using JetBrains.Annotations;
using SharedLibrary.Models;
using UnityEngine;
using System.Threading.Tasks;
using SharedLibrary.Contracts.Hubs;
using LocalManagers.ConnectToGame;

namespace Scripts.RegisterLoginScripts
{
	public class NetworkingManager : BehaviorPersistentSingleton<NetworkingManager>
	{
		private string BaseURL = @"https://localhost:7148/";

		#region ParamsStore

		public Guid LobbyId { get; set; }

		//TODO: Review in the future
		public string LobbyName { get; set; }


		#region Access Token

		private string _accessToken = null;
		public string AccessToken
		{
			get => _accessToken;
			set
			{
				_accessToken = value;
				Debug.Log($"Token set to: {_accessToken}");
			}
		}

		#endregion

		#region Selected in "Connect to game" lobby id

		private string _selectedLobbyId;

		public string SelectedLobbyId
		{
			get => _selectedLobbyId;
			set
			{
				_selectedLobbyId = value;

				if (Debug.isDebugBuild)
					Debug.Log($"lobby with id \"{_selectedLobbyId}\" was selected");
			}
		}

		#endregion

		public string HeroName { get; set; }

		public string LobbyToCreateName { get; set; }

		public IList<Lobby> Lobbies { get; set; }// = new List<Lobby>(0);

		#endregion

		#region SignalR

		public HubConnection HubConnection { get; set; } = null;

		public void StartHub()
		{
			HubConnection = new HubConnectionBuilder()
				.WithUrl($"{BaseURL}/hubs/lobby", options =>
				{
					options.AccessTokenProvider = () => Task.FromResult(AccessToken);
				}) //Add substr
				.WithAutomaticReconnect()
				.Build();

			HubConnection.On<string>(ClientHandlers.Lobby.Error,
				(string errorMessage) =>
				{
					if (Debug.isDebugBuild) Debug.Log($"an error ocured. error message: {errorMessage}");
					InformationPanelController.Instance.CreateMessage(InformationPanelController.MessageType.ERROR,
						errorMessage);
				});

			HubConnection.On<string>(ClientHandlers.Lobby.DeleteLobbyHandler,
				(string serverMessage) =>
				{
					InformationPanelController.Instance.CreateMessage(InformationPanelController.MessageType.INFO,
						serverMessage);
				});

			HubConnection.On<Lobby>(ClientHandlers.Lobby.ConnectToLobbyHandler,
				(Lobby lobby) =>
				{
					PlayersListController.Instance.UpdatePlayersList(lobby);
				});

			HubConnection.On<Lobby>(ClientHandlers.Lobby.ChangeReadyStatus,
				(Lobby lobby) =>
				{
					PlayersListController.Instance.UpdatePlayersList(lobby);
				});

			HubConnection.On<Lobby>(ClientHandlers.Lobby.ExitFromLobbyHandler,
				(Lobby lobby) =>
				{
					PlayersListController.Instance.UpdatePlayersList(lobby);
				});

			HubConnection.On<Lobby>(ClientHandlers.Lobby.ChangeLobbyDataHandler,
				(Lobby lobby) =>
				{
					throw new NotImplementedException();
				});

			HubConnection.On<Lobby>(ClientHandlers.Lobby.ChangedColor,
				(Lobby lobby) =>
				{
					throw new NotImplementedException();
				});

			HubConnection.On<Hero>(ClientHandlers.Lobby.CreatedSessionHandler,
				(Hero hero) =>
				{
					throw new NotImplementedException();
				});

			HubConnection.StartAsync().Wait();
		}

		public async void StopHub()
		{
			if (HubConnection is not null)
			{
				await HubConnection.StopAsync();
			}
			else
			{
				if (Debug.isDebugBuild)
				{
					Debug.Log("Hub is not running so it can`t been stopped");
				}
			}
		}

		#endregion

		#region REST

		public bool Sending { get; set; } = false;

		public IEnumerator Routine_SendDataToServer<T>(RestRequestForm<T> requestForm)
			where T : ResponseBase
		{
			using UnityWebRequest request = new UnityWebRequest($"{BaseURL}{requestForm.EndPoint}",
				requestForm.RequestType.ToString());
			request.SetRequestHeader("Content-Type", "application/json");

			if (requestForm.JsonData is not null)
			{
				byte[] rowData = Encoding.UTF8.GetBytes(requestForm.JsonData);
				request.uploadHandler = new UploadHandlerRaw(rowData);
			}

			request.downloadHandler = new DownloadHandlerBuffer();

			if (!String.IsNullOrWhiteSpace(requestForm.Token))
			{
				AttachHeader(request, "Authorization", $"{requestForm.Token}");
			}

			yield return request.SendWebRequest();

			requestForm.Result = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);

			requestForm.ResponseHandler.ProcessResponse(request, requestForm);
		}

		#endregion

		private void AttachHeader(UnityWebRequest request, string key, string value)
		{
			request.SetRequestHeader(key, value);
		}
	}

	public enum RequestType
	{
		GET = 0,
		POST = 1,
		PUT = 2
	}
}