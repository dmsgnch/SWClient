using Assets.Scripts.Components.Abstract;
using Assets.Scripts.Managers;
using LocalManagers.ConnectToGame;
using LocalManagers.ConnectToGame.Requests;
using LocalManagers.RegisterLoginRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using ViewModels.Abstract;

namespace Assets.Scripts.ViewModels
{
	public class ConnectToGameViewModel : ViewModelBase
	{
		CreateLoginRequest createLoginRequest;

		public ConnectToGameViewModel()
		{ }

		public void UpdateLobbiesList(GameObject lobbiesListItemPrefab, Button connectToGameButton)
		{
			new GetLobbiesListRequest().GetLobbyList();

			LobbiesListController.Instance.UpdateLobbiesListDisplay(
				GameManager.Instance.MainDataStore.Lobbies, 
				lobbiesListItemPrefab, 
				connectToGameButton);
		}

		public void OnToLobbyButtonClick()
		{
			GameManager.Instance.ChangeState(GameState.Lobby);
		}

		private void OnCoroutineFinishedEventHandler()
		{
			Destroy(createLoginRequest.gameObject);
		}

		public void CloseApplication()
		{
			if (Debug.isDebugBuild)
				Debug.Log("Application quiting");

			Application.Quit();
		}
	}
}
