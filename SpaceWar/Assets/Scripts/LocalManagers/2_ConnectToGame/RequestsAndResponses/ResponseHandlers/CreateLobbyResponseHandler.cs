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
using Scripts.RegisterLoginScripts;
using SharedLibrary.Models;

namespace Assets.Scripts.LocalManagers._0_RegisterLoginRequests.ResponseHandlers
{
	public class CreateLobbyResponseHandler : IResponseHandler
	{
		public void BodyConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
			where T : ResponseBase
		{
			//Not create information panel
		}

		public async void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
			where T : ResponseBase
		{
			Lobby lobby = requestForm.GetResponseResult<CreateLobbyResponse>().Lobby;

			GameManager.Instance.LobbyDataStore.LobbyId = lobby.Id;
			GameManager.Instance.LobbyDataStore.IsLobbyLeader = lobby.LobbyInfos.First(l => l.UserId.Equals(
				GameManager.Instance.MainDataStore.UserId)).LobbyLeader;

			GameManager.Instance.ChangeState(GameState.Lobby);

			UnityEngine.Object.FindAnyObjectByType<LobbyView>()?.UpdatePlayersListInLobby(lobby);
			await NetworkingManager.Instance.StartHub("lobby");
		}

		public void OnRequestFinished()
		{
			UnityEngine.Object.Destroy(ConnectToGameViewModel.CreateLobbyRequestSenderObject);
		}
	}
}
