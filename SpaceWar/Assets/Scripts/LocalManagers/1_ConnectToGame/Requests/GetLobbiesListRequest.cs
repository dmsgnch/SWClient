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
    public class GetLobbiesListRequest : NetworkingManager
    {
        [SerializeField] private GameObject lobbiesPanel;
        [SerializeField] private GameObject lobbyTemplate;

        private static GameObject _lobbiesPanel;
        private static GameObject _lobbyTemplate;

        private const string ConnectionEndpoint = "Lobby/GetAll";

        [SerializeField] private GameObject parentCanvasObject;

        void Start()
        {
            _lobbiesPanel = lobbiesPanel;
            _lobbyTemplate = lobbyTemplate;

            Button btn = gameObject.GetComponent<Button>();
            btn.onClick.AddListener(GetLobbyList);
        }

        void OnEnable()
        {
            GetLobbyList();
        }

        void GetLobbyList()
        {
            RestRequestForm<GetAllLobbiesResponse> requestForm =
                new RestRequestForm<GetAllLobbiesResponse>(ConnectionEndpoint,
                    RequestType.GET, parentCanvasObject, new GetAllLobbiesResponseHandler(), token: AccessToken);

            var result = StartCoroutine(Routine_SendDataToServer(requestForm));
        }

        static void DisplayLobbiesList(IList<Lobby> lobbies)
        {
            for (int i = 0; i < lobbies.Count; i++)
            {
                var element = Instantiate(_lobbyTemplate, _lobbiesPanel.transform);
                element.transform.GetChild(0).GetComponent<Text>().text += lobbies[i].LobbyName;
                element.SetActive(true);
            }
        }
        private class GetAllLobbiesResponseHandler : IResponseHandler
        {
            public GameObject infoPanelCanvas { get; set; }

            public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
                where T : ResponseBase
            {
                //TODO: Review code below
                if (requestForm.Result is not GetAllLobbiesResponse) throw new ArgumentException(); 
                GetAllLobbiesResponse response = requestForm.Result as GetAllLobbiesResponse;

                new Task(() => DisplayLobbiesList(response.Lobbies));
            }
        }
    }
}