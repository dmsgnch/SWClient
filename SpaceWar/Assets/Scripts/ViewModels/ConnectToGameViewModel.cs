using Assets.Scripts.Components.Abstract;
using Assets.Scripts.Managers;
using LocalManagers.ConnectToGame.Requests;
using LocalManagers.RegisterLoginRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using ViewModels.Abstract;

namespace Assets.Scripts.ViewModels
{
	public class ConnectToGameViewModel : ViewModelBase
	{
		CreateLoginRequest createLoginRequest;

		public ConnectToGameViewModel()
		{ }

		public void OnUpdateButtonClick()
		{
			new GetLobbiesListRequest().GetLobbyList();

			UpdateLobbiesListDisplay(GameManager.Instance.MainDataStore.Lobbies);
			//createLoginRequest = new GameObject().AddComponent<CreateLoginRequest>();

			//createLoginRequest.onCoroutineFinished.AddListener(OnCoroutineFinishedEventHandler);

			//createLoginRequest.CreateRequest();
		}

		public void OnToRegisterButtonClick()
		{
			// TODO: Need review
			GameManager.Instance.uiManager.Hide<LoginViewModel>();
			GameManager.Instance.uiManager.BindAndShow(GameManager.Instance.RegisterViewModel);
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
