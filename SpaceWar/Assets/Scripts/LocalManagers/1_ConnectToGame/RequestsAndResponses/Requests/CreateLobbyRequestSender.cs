using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Components;
using Assets.Scripts.Managers;
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
using Assets.Scripts.ViewModels;
using Assets.Scripts.LocalManagers._0_RegisterLoginRequests.ResponseHandlers;

namespace LocalManagers.ConnectToGame.Requests
{
	/// <summary>
	/// 
	/// </summary>
	public class CreateLobbyRequestSender : MonoBehaviour
	{
		private const string ConnectionEndpoint = "Lobby/Create";

		public void CreateLobby()
		{
			CreateLobbyRequest request = new CreateLobbyRequest(GameManager.Instance.ConnectToGameDataStore.LobbyName);

			RestRequestForm<CreateLobbyResponse> requestForm =
				new RestRequestForm<CreateLobbyResponse>(ConnectionEndpoint,
					RequestType.POST, new GetLobbyResponseHandler(),
					token: GameManager.Instance.MainDataStore.AccessToken, jsonData: JsonConvert.SerializeObject(request));

			var result = StartCoroutine(NetworkingManager.Instance.Routine_SendDataToServer<CreateLobbyResponse>(requestForm));
		}
	}
}