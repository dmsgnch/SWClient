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

		private void OnQuitButtonClick()
		{
			_lobbyViewModel.ExitFromLobby();
		}

		private void Init()
		{
			_lobbyViewModel.DefineButton(startButton, readyButton);

			PlayersListController.Instance.UpdatePlayersList(_lobbyViewModel.GetSampleData());

			quitButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(_lobbyViewModel.ExitFromLobby);
		}

		protected override void OnBind(LobbyViewModel lobbyViewModel)
		{
			_lobbyViewModel = lobbyViewModel;

			Init();
		}
	}
}