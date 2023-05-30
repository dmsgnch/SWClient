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
using Assets.Scripts.Components;
using Assets.Scripts.Managers;
using static Assets.Scripts.ViewModels.ConnectToGameViewModel;
using Assets.Scripts.LocalManagers._0_RegisterLoginRequests.ResponseHandlers;

namespace LocalManagers.ConnectToGame.Requests
{
    public class GetLobbiesListRequestSender : MonoBehaviour
    {
        private const string ConnectionEndpoint = "Lobby";

        public void GetLobbyList()
        {           
            RestRequestForm<GetAllLobbiesResponse> requestForm =
                new RestRequestForm<GetAllLobbiesResponse>(ConnectionEndpoint, 
                    RequestType.GET, new GetAllLobbiesResponseHandler(), 
                    token: GameManager.Instance.MainDataStore.AccessToken);

            var result = StartCoroutine(
                NetworkingManager.Instance.Routine_SendDataToServer(requestForm));
        }       
        
    }
}