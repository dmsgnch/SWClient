using Assets.Scripts.Managers;
using Components.Abstract;
using Components;
using LocalManagers.RegisterLoginRequests;
using SharedLibrary.Responses.Abstract;
using SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.ViewModels;
using LocalManagers.ConnectToGame.Requests;
using Assets.Scripts.View;

namespace Assets.Scripts.LocalManagers._0_RegisterLoginRequests.ResponseHandlers
{
	public class CreateLobbyResponseHandler : IResponseHandler
	{
		public void BodyConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
			where T : ResponseBase
		{
			//Not create information panel
		}

		public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
			where T : ResponseBase
		{
			GameManager.Instance.LobbyDataStore.LobbyId =
				requestForm.GetResponseResult<CreateLobbyResponse>().Lobby.Id;

			GameManager.Instance.ChangeState(GameState.Lobby);

			UnityEngine.Object.FindAnyObjectByType<LobbyView>()?.UpdatePlayersListInLobby(requestForm.GetResponseResult<CreateLobbyResponse>().Lobby);
		}

		public void OnRequestFinished()
		{
			UnityEngine.Object.Destroy(ConnectToGameViewModel.CreateLobbyRequestSenderObject);
		}
	}
}
