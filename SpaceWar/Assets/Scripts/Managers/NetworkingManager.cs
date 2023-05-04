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

namespace Scripts.RegisterLoginScripts
{
    public class NetworkingManager : GetterPersistentSingleton<NetworkingManager>
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

        #endregion

        #region SignalR

        // public HubConnection HubConnection { get; set; }
        //
        // private void Start()
        // {
        //     HubConnection = new HubConnectionBuilder()
        //         .WithAutomaticReconnect()
        //         .WithUrl($"{BaseURL}") //Add substr
        //         .Build();
        //
        //     HubConnection.On<string, string>("Receive", (user, message) =>
        //     {
        //         var newMessage = $"{user}: {message}";
        //     });
        // }

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