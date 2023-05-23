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
using Assets.Scripts.Components;
using UnityEngine.Events;
using Assets.Scripts.Managers;

namespace Scripts.RegisterLoginScripts
{
	public class NetworkingManager : ComponentSingleton<NetworkingManager>
	{
		private const string BaseURL = @"https://localhost:44355/";

		#region SignalR

		public HubConnection HubConnection { get; set; } = null;

		public void StartHub(string endpoint)
		{
			HubConnection = new HubConnectionBuilder()
				// /hubs/lobby
				.WithUrl($"{BaseURL}{endpoint}", options =>
				{
					options.AccessTokenProvider = () => Task.FromResult(GameManager.Instance.MainDataStore.AccessToken);
				}) 
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
		PUT = 2,
		DELETE = 3
	}
}