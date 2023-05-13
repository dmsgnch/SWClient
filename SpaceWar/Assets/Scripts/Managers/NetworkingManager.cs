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
using Assets.Scripts.SignalR;
using LocalManagers.ConnectToGame;
using System.Linq;
using Assets.Scripts.LocalManagers._1_ConnectToGame;

namespace Scripts.RegisterLoginScripts
{
    public class NetworkingManager : BehaviorPersistentSingleton<NetworkingManager>
    {
        private const string BaseURL = @"https://localhost:7148/";

        #region ParamsStore

        public string AccessToken { get; set; } = "token";

        public Guid LobbyId { get; set; }

        public string LobbyName { get; set; }

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

        public string HeroName { get; set; }

        public string LobbyToCreateName { get; set; }

        public IList<Lobby> Lobbies { get; set; }// = new List<Lobby>(0);

        public LobbyInfo LobbyInfo { get; private set; }

        #endregion

        #region SignalR

        public HubConnection HubConnection { get; set; }

        private void Start()
        {
            DisplaySampleLobby();

            HubConnection = new HubConnectionBuilder()
                .WithUrl($"{BaseURL}", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(AccessToken);
                }) //Add substr
                .WithAutomaticReconnect()
                .Build();

            HubConnection.On<string>(ClientHandlers.Lobby.Error,
                SignalRHandler.Instance.Error);

            HubConnection.On<string>(ClientHandlers.Lobby.DeleteLobbyHandler,
                SignalRHandler.Instance.DeleteLobby);

            HubConnection.On<Lobby>(ClientHandlers.Lobby.ConnectToLobbyHandler,
                SignalRHandler.Instance.ConnectToLobby);

            HubConnection.On<Lobby>(ClientHandlers.Lobby.ChangeReadyStatus,
                SignalRHandler.Instance.ChangeReadyStatus);

            HubConnection.On<Lobby>(ClientHandlers.Lobby.ExitFromLobbyHandler,
                SignalRHandler.Instance.ExitFromLobby);

            HubConnection.On<Lobby>(ClientHandlers.Lobby.ChangeLobbyDataHandler,
                SignalRHandler.Instance.ChangeLobbyData);

            HubConnection.On<Lobby>(ClientHandlers.Lobby.ChangedColor,
                SignalRHandler.Instance.ChangedColor);

            HubConnection.On<Hero>(ClientHandlers.Lobby.CreatedSessionHandler,
                SignalRHandler.Instance.CreateSession);

            //HubConnection.StartAsync().Wait();
        }

        private void DisplaySampleLobby()
        {
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
                            Id = 1,
                        },
                        LobbyLeader = true,
                        UserId = 1,
                    },
                    new LobbyInfo
                    {
                        User = new ApplicationUser
                        {
                            Username = "not a host",
                            Id = 2,
                        },
                        LobbyLeader = false,
                        UserId = 2,
                    },
                },
            };
            LobbyInfo = lobby.LobbyInfos.First();
            LobbyInfo.Lobby = lobby;

            PlayersListController.Instance.UpdatePlayersList(lobby);
            StartButtonController.Instance.DefineButton();
        }

        #endregion

        #region REST

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