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

		private void Awake()
		{
			quitButton.onClick.AddListener(OnQuitButtonClick);
		}

		private async void OnQuitButtonClick()
		{
			await _lobbyViewModel.ExitFromLobby();
		}

		private void OnEnable()
		{
			if (_lobbyViewModel is null) return;

			_lobbyViewModel.DefineButton(startButton, readyButton);

			PlayersListController.Instance.UpdatePlayersList(_lobbyViewModel.GetSampleData());
		}

		protected override void OnBind(LobbyViewModel lobbyViewModel)
		{
			_lobbyViewModel = lobbyViewModel;
		}
	}
}