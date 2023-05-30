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
using System.Linq;
using System.Text.Json;
using Assets.Scripts.View;
using SharedLibrary.Responses;

namespace Scripts.RegisterLoginScripts
{
	public class NetworkingManager : ComponentSingleton<NetworkingManager>
	{
		private const string BaseURL = @"https://localhost:7148/";

		#region SignalR

		public HubConnection HubConnection { get; set; } = null;

		private void CreateHub(string hubName)
		{
			HubConnection = new HubConnectionBuilder()
				.WithUrl($"{BaseURL}hubs/{hubName}", options =>
				{
					options.AccessTokenProvider = () => Task.FromResult(GameManager.Instance.MainDataStore.AccessToken);
				})
				.WithAutomaticReconnect()
				.Build();

			ConfigureHandlers(HubConnection);
		}
		private Lobby ConfigureHandlers(HubConnection hubConnection)
		{
			Lobby currentLobby1 = null;

			hubConnection.On<string>(ClientHandlers.Lobby.Error, (errorMessage) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					if (Debug.isDebugBuild) Debug.Log($"an error ocured. error message: {errorMessage}");
					InformationPanelController.Instance.CreateMessage(InformationPanelController.MessageType.ERROR,
						errorMessage);
				});
			});

			hubConnection.On<string>(ClientHandlers.Lobby.DeleteLobbyHandler, (serverMessage) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
                    GameManager.Instance.ChangeState(GameState.ConnectToGame);

                    InformationPanelController.Instance.CreateMessage(
						InformationPanelController.MessageType.WARNING,
						serverMessage);
                });
			});

			hubConnection.On<Lobby>(ClientHandlers.Lobby.ConnectToLobbyHandler, (lobby) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					Debug.Log($"Game state on start: {GameManager.Instance.State}");
					if (GameManager.Instance.State is GameState.ConnectToGame)
					{
						Debug.Log($"State is changing to lobby");
						GameManager.Instance.LobbyDataStore.LobbyId = lobby.Id;
						GameManager.Instance.ChangeState(GameState.Lobby);
					}

					Debug.Log($"Game state before updating: {GameManager.Instance.State}");
					if (GameManager.Instance.State is GameState.Lobby)
					{
						PlayersListController.Instance.UpdatePlayersList(lobby);
					}
				});
			});

			hubConnection.On<Lobby>(ClientHandlers.Lobby.ExitFromLobbyHandler, (lobby) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					if (lobby.LobbyInfos.Any(l => l.UserId.Equals(GameManager.Instance.MainDataStore.UserId)))
					{
						FindAnyObjectByType<LobbyView>()?.UpdatePlayersListInLobby(lobby);
					}
					else
					{
						GameManager.Instance.ChangeState(GameState.ConnectToGame);
					}
				});
			});

			hubConnection.On<Lobby>(ClientHandlers.Lobby.ChangeLobbyDataHandler, (lobby) =>
			{
				throw new NotImplementedException();
			});

			hubConnection.On<LobbyInfo>(ClientHandlers.Lobby.ChangedColor, (info) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					PlayersListController.Instance.ChangeColor(info);
				});
			});

			hubConnection.On<LobbyInfo>(ClientHandlers.Lobby.ChangeReadyStatus, (info) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
                    PlayersListController.Instance.ChangeReadyStatus(info);
					FindAnyObjectByType<LobbyView>().SetInteractableStartButton();
				});
			});

			hubConnection.On<Guid>(ClientHandlers.Lobby.CreatedSessionHandler, (sessionId) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					GameManager.Instance.SessionDataStore.SessionId = sessionId;

					GameManager.Instance.ChangeState(GameState.LoadMainGameScene);
				});
			});

			hubConnection.On<HeroMapView>(ClientHandlers.Session.ResearchedPlanet, (heroMap) =>
			{
				throw new NotImplementedException();
			});

			hubConnection.On<HeroMapView>(ClientHandlers.Session.ReceiveHeroMap, (heroMap) =>
			{
				throw new NotImplementedException();
			});

			hubConnection.On<string>(ClientHandlers.ErrorHandler, HandleStringMessageOutput());
			hubConnection.On<string>(ClientHandlers.Session.StartedResearching, HandleStringMessageOutput());
			hubConnection.On<string>(ClientHandlers.Session.StartedColonizingPlanet, HandleStringMessageOutput());
			hubConnection.On<string>(ClientHandlers.Session.IterationDone, HandleStringMessageOutput());
			hubConnection.On<string>(ClientHandlers.Session.PostResearchOrColonizeErrorHandler, HandleStringMessageOutput());
			hubConnection.On<string>(ClientHandlers.Session.HealthCheckHandler, HandleStringMessageOutput());

			return currentLobby1;
		}

		private Action<string> HandleStringMessageOutput()
		{
			return (message) =>
			{
				Debug.Log(message);
			};
		}

		public async Task StartHub(string endpoint)
		{
			if (HubConnection is null)
			{
				CreateHub(endpoint);
			}

			if (HubConnection.State is HubConnectionState.Disconnected)
			{
				await HubConnection.StartAsync();
			}
		}

		public async Task StopHub()
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

			if (requestForm.JsonData is not null)
			{
				request.SetRequestHeader("Content-Type", "application/json");
				byte[] rowData = Encoding.UTF8.GetBytes(requestForm.JsonData);
				request.uploadHandler = new UploadHandlerRaw(rowData);
			}

			request.downloadHandler = new DownloadHandlerBuffer();

			if (!String.IsNullOrWhiteSpace(requestForm.Token))
			{
				AttachHeader(request, "Authorization", $"Bearer {requestForm.Token}");
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