using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Components.Abstract;
using Newtonsoft.Json;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Models;
using SharedLibrary.Responses;
using SharedLibrary.Responses.Abstract;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Components;

namespace LocalManagers.ConnectToGame.Requests
{
    public class GetLobbiesListRequest
    {
        private const string ConnectionEndpoint = "Lobby/GetAll";

        public void GetLobbyList()
        {
            Debug.Log($"Token: {NetworkingManager.AccessToken}");
            RestRequestForm<GetAllLobbiesResponse> requestForm =
                new RestRequestForm<GetAllLobbiesResponse>(ConnectionEndpoint, 
                    RequestType.GET, new GetAllLobbiesResponseHandler(), NetworkingManager.AccessToken);

            var result = NetworkingManager.Instance.StartCoroutine(NetworkingManager.Instance.Routine_SendDataToServer(requestForm));
        }
        
        private class GetAllLobbiesResponseHandler : IResponseHandler
        {
            public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
                where T : ResponseBase
            {
                //TODO: Review code below
                if (requestForm.Result is not GetAllLobbiesResponse) throw new ArgumentException(); 
                GetAllLobbiesResponse response = requestForm.Result as GetAllLobbiesResponse;

                NetworkingManager.Instance.Lobbies = response.Lobbies;
            }
        }
    }
}