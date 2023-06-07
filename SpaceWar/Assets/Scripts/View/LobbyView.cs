using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using ViewModels.Abstract;
using Assets.View.Abstract;
using Assets.Scripts.ViewModels;
using LocalManagers.ConnectToGame;
using SharedLibrary.Responses;
using SharedLibrary.Models;

namespace Assets.Scripts.View
{
	public class LobbyView : AbstractScreen<LobbyViewModel>
	{
		[SerializeField] private GameObject playerList;
		[SerializeField] private GameObject playerListItemPrefab;
		[SerializeField] private Button quitButton;

		[SerializeField] private GameObject startButton;
		[SerializeField] private GameObject readyButton;

		private LobbyViewModel _lobbyViewModel;

		#region Unity methods

		private void Awake()
		{
			quitButton.onClick.AddListener(OnQuitButtonClick);
		}

		private void OnEnable()
		{
			if (_lobbyViewModel is null) return;

			_lobbyViewModel.DefineButton(this, startButton, readyButton);

			SetInteractableStartButton();
		}

		#endregion

		#region Buttons handlers

		private async void OnQuitButtonClick()
		{
			PlayButtonClickSound();

			await _lobbyViewModel.SendExitFromLobbyRequest();
		}

		#endregion

		#region Commands

		public void UpdatePlayersListInLobby(Lobby lobby)
		{
			PlayersListController.Instance.UpdatePlayersList(lobby);

			SetInteractableStartButton();
		}

		#endregion

		#region Ui controllers

		public void SetInteractableStartButton()
		{
			if(startButton.activeInHierarchy) _lobbyViewModel.SetInteractableStartButton(startButton);
		}
		
		#endregion

		protected override void OnBind(LobbyViewModel lobbyViewModel)
		{
			_lobbyViewModel = lobbyViewModel;
		}
	}
}