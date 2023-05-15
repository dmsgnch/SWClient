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
	/// 
	/// </summary>
	public class CreateLobbyRequestSender : NetworkingManager
	{
		private const string ConnectionEndpoint = "Lobby/Create";

		void Start()
		{
			Button btn = gameObject.GetComponent<Button>();
			btn.onClick.AddListener(CreateLobby);
		}

		void CreateLobby()
		{
			CreateLobbyRequest request = new CreateLobbyRequest(LobbyName);

			RestRequestForm<CreateLobbyResponse> requestForm =
				new RestRequestForm<CreateLobbyResponse>(ConnectionEndpoint,
					RequestType.POST, new CreateLobbyRequestSender.GetLobbyResponseHandler(),
					token: AccessToken, jsonData: JsonConvert.SerializeObject(request));

			var result = StartCoroutine(Routine_SendDataToServer<CreateLobbyResponse>(requestForm));
		}

		private class GetLobbyResponseHandler : IResponseHandler
		{
			public void BodyConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
				where T : ResponseBase
			{
				//Not create information panel
			}

			public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
				where T : ResponseBase
			{
				NetworkingManager.Instance.LobbyId = requestForm.GetResponseResult<CreateLobbyResponse>().Lobby.Id;

				ChangeActiveObjects.Instance.SwapActivity("cnvs_LobbiesList", "cnvs_Lobby");
			}
		}
	}
}