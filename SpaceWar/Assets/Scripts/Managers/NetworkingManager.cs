using System;
using System.Collections;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using SharedLibrary.Responses.Abstract;
using UnityEngine.Networking;
using Components;
using SharedLibrary.Models;
using UnityEngine;
using SharedLibrary.Contracts.Hubs;
using LocalManagers.ConnectToGame;
using Assets.Scripts.Managers;
using System.Linq;
using Assets.Scripts.View;
using Task = System.Threading.Tasks.Task;
using SharedLibrary.Responses;
using Unity.VisualScripting;
using Edge = SharedLibrary.Models.Edge;
using static Assets.Scripts.Components.DataStores.SessionDataStore;

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

			if (hubName == "session" || hubName == "Session")
			{
				ConfigureHandlersSession(HubConnection);
			}
			else
			{
				ConfigureHandlersLobby(HubConnection);
			}
		}

		private Lobby ConfigureHandlersLobby(HubConnection hubConnection)
		{
			Lobby currentLobby1 = null;

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
						FindAnyObjectByType<LobbyView>()?.UpdatePlayersListInLobby(lobby);
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
					GetView<LobbyView>().SetInteractableStartButton();
				});
			});

			hubConnection.On<LobbyInfo>(ClientHandlers.Lobby.ChangeReadyStatus, (info) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					PlayersListController.Instance.ChangeReadyStatus(info);
					GetView<LobbyView>().SetInteractableStartButton();
				});
			});

			hubConnection.On<Guid>(ClientHandlers.Lobby.CreatedSessionHandler, (sessionId) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					GameManager.Instance.SessionDataStore.SessionId = sessionId;

                    StopHub();
					GameManager.Instance.ChangeState(GameState.LoadMainGameScene);
                });
			});

			hubConnection.On<string>(ClientHandlers.Lobby.Error, HandleStringMessageOutput());

			return currentLobby1;
		}

		private Lobby ConfigureHandlersSession(HubConnection hubConnection)
		{
			Lobby currentLobby1 = null;

			hubConnection.On<HeroMapView>(ClientHandlers.Session.ReceiveHeroMap, (heroMap) =>
			{
				throw new NotImplementedException();
			});

			hubConnection.On<Session>(ClientHandlers.Session.ReceiveSession, (session) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					var hudView = GetView<HUDView>();

					if (!GameManager.Instance.SessionDataStore.SessionId.Equals(session.Id))
						throw new ArgumentException();

					GameManager.Instance.SessionDataStore.TurnNumber = session.TurnNumber;
					GameManager.Instance.SessionDataStore.TurnTimeLimit = session.TurnTimeLimit;
                    GameManager.Instance.SessionDataStore.CurrentHeroTurnId = session.HeroTurnId;
                    GameManager.Instance.SessionDataStore.PanelHeroForms = 
					session.Heroes.Select(
					h => new PanelHeroForm()
					{
					    HeroId = h.HeroId,
					    HeroName = h.Name
					}).ToList();

                    hudView.UpdateSessionHudValues();

					hudView.UpdatePlayersListPanelValues();
				});
			});

			hubConnection.On<UpdatedFortificationResponse>(ClientHandlers.Session.UpdatedFortification, (fortificationResponse) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					var planetsView = GetView<PlanetsView>();

					Planet updatedPlanet = GameManager.Instance
					.HeroDataStore.HeroMapView.Planets
					.FirstOrDefault(p => p.Id.Equals(fortificationResponse.PlanetId));

					updatedPlanet.FortificationLevel = fortificationResponse.Fortification;
					updatedPlanet.IterationsLeftToNextStatus = fortificationResponse.IterationsToTheNextStatus;

                    GameManager.Instance.HeroDataStore.Resourses = fortificationResponse.Resources;
					GameManager.Instance.HeroDataStore.AvailableColonizationShips = fortificationResponse.AvailableColonizationShips;
					GameManager.Instance.HeroDataStore.AvailableResearchShips = fortificationResponse.AvailableResearchShips;

					planetsView.UpdatePlanet(updatedPlanet);
				});
			});

			hubConnection.On<Battle>(ClientHandlers.Session.ReceiveBattle, (battle) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					var planetsView = GetView<PlanetsView>();

					if (!GameManager.Instance.BattleDataStore.Battles.Any(b => b.Id.Equals(battle.Id)))
					{
						GameManager.Instance.BattleDataStore.Battles.Add(battle);
					}

					Edge[] connections = GameManager.Instance.HeroDataStore.HeroMapView.Connections.ToArray();
					Edge battleConnection = connections.FirstOrDefault(c =>
						(c.FromPlanetId.Equals(battle.AttackedPlanetId) || c.FromPlanetId.Equals(battle.AttackedFromId)) &&
						(c.ToPlanetId.Equals(battle.AttackedFromId) || c.ToPlanetId.Equals(battle.AttackedPlanetId))
					);

                    planetsView.UpdateConnection(battleConnection);
                });
			});

			hubConnection.On<NextTurnResponse>(ClientHandlers.Session.NextTurnHandler, HandleUpdateAllParams());

			hubConnection.On<UpdatedPlanetStatusResponse>(
				ClientHandlers.Session.StartPlanetResearchingOrColonization, 
				(newPlanetStatus) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					var planetsView = GetView<PlanetsView>();
					var hudView = GetView<HUDView>();

                    Planet updatedPlanet = GameManager.Instance
                    .HeroDataStore.HeroMapView.Planets
                    .FirstOrDefault(p => p.Id.Equals(newPlanetStatus.PlanetId));
					updatedPlanet.Status = newPlanetStatus.RelationStatus;
					updatedPlanet.IterationsLeftToNextStatus = newPlanetStatus.IterationsToTheNextStatus;

					GameManager.Instance.HeroDataStore.Resourses = newPlanetStatus.Resources;
					GameManager.Instance.HeroDataStore.AvailableColonizationShips = 
					newPlanetStatus.AvailableColonizationShips;
					GameManager.Instance.HeroDataStore.AvailableResearchShips = 
					newPlanetStatus.AvailableResearchShips;

					planetsView.UpdatePlanet(updatedPlanet);
				});
			});

			hubConnection.On<NextTurnResponse>(ClientHandlers.Session.GetHeroDataHandler, 
				HandleUpdateAllParams());

			hubConnection.On<string>(ClientHandlers.Session.PostResearchOrColonizeErrorHandler,
				HandleStringMessageOutput());

			return currentLobby1;
		}

		private Action<string> HandleStringMessageOutput()
		{
			return (message) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					Debug.Log(message);

					InformationPanelController.Instance.CreateMessage(InformationPanelController.MessageType.WARNING, message);
				});
			};
		}

		private Action<NextTurnResponse> HandleUpdateAllParams()
		{
			return (data) =>
			{
				UnityMainThreadDispatcher.Instance().Enqueue(() =>
				{
					var hudView = GetView<HUDView>();
					var planetsView = GetView<PlanetsView>();

					GameManager.Instance.HeroDataStore.HeroMapView = data.HeroMapView;

					hudView.SetHeroNewValues(data.Hero);

					GameManager.Instance.SessionDataStore.TurnNumber = data.Session.TurnNumber;
					GameManager.Instance.SessionDataStore.TurnTimeLimit = data.Session.TurnTimeLimit;
					GameManager.Instance.SessionDataStore.CurrentHeroTurnId = data.Session.HeroTurnId;
					GameManager.Instance.BattleDataStore.Battles = data.Battles;

					hudView.UpdateHeroHudValues();
					hudView.UpdateSessionHudValues();
                    hudView.UpdateHeroRequest();
                    hudView.UpdatePlayersListPanelValues();

                    //recreate map
                    planetsView.GeneratePlanetsWithConnections();
				});
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

		private T GetView<T>() where T : Component
		{
			return FindFirstObjectByType<T>() ?? throw new Exception();
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