using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Components;
using Components.Abstract;
using Newtonsoft.Json;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Models;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Responses.Abstract;
using UnityEngine;
using UnityEngine.UI;

namespace LocalManagers.ConnectToGame.Requests
{
    /// <summary>
    /// This object is temp, because I must implement SignalR listener with big Lobby manager
    /// </summary>
    public class CreateLobbyIdRequest : NetworkingManager
    {
        private const string ConnectionEndpoint = "GetAll";

        [SerializeField] private GameObject parentCanvasObject;

        void OnEnable()
        {
            CreateLobby();
        }

        void CreateLobby()
        {
            CreateLobbyRequest request = new CreateLobbyRequest(LobbyName);
            
            RestRequestForm<CreateLobbyResponse> requestForm =
                new RestRequestForm<CreateLobbyResponse>(ConnectionEndpoint,
                    RequestType.POST, parentCanvasObject, new CreateLobbyIdRequest.GetLobbyResponseHandler(), 
                    JsonConvert.SerializeObject(request), AccessToken);
            
           var result = StartCoroutine(Routine_SendDataToServer<CreateLobbyResponse>(requestForm));
        }
        
        private class GetLobbyResponseHandler : IResponseHandler
        {
            public GameObject infoPanelCanvas { get; set; }

            public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
                where T : ResponseBase
            {
                //TODO: Review code below
                if (requestForm.Result is not CreateLobbyResponse) throw new ArgumentException(); 
                CreateLobbyResponse response = requestForm.Result as CreateLobbyResponse;

                LobbyId = response.Lobby.Id;
                
                ChangeActiveObjects.Instance.SwapActivity("cnvs_LobbiesList", "cnvs_Lobby");
            }
        }
    }
}