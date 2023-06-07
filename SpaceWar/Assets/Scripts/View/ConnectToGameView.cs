using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using ViewModels.Abstract;
using View.Abstract;
using Assets.View.Abstract;
using Assets.Scripts.ViewModels;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Assets.Scripts.Managers;
using LocalManagers.ConnectToGame;

namespace Assets.Scripts.View
{
	public class ConnectToGameView : AbstractScreen<ConnectToGameViewModel>
	{
		#region NotifyPropertyChanged

		//private InputField _heroInput;

		//public InputField HeroInput
		//{
		//	get => _heroInput;
		//	set => Set(ref _heroInput, value);
		//}

		//private InputField _gameNameInput;

		//public InputField GameNameInput
		//{
		//	get => _gameNameInput;
		//	set => Set(ref _gameNameInput, value);
		//}

		#endregion

		[SerializeField] private InputField heroInput;
		[SerializeField] private InputField gameNameInput;

		[SerializeField] private UnityEngine.UI.Button toMainMenuButton;
		[SerializeField] private UnityEngine.UI.Button updateButton;
		[SerializeField] private UnityEngine.UI.Button connectToGameButton;
		[SerializeField] private UnityEngine.UI.Button createGameButton;
		[SerializeField] private GameObject lobbiesListItemPrefab;

		[SerializeField] protected UnityEngine.UI.Image _heroIcon;
		[SerializeField] protected UnityEngine.UI.Image _gameIcon;
		[SerializeField] protected Sprite _validSprite;
		[SerializeField] protected Sprite _errorSprite;

		private ConnectToGameViewModel _connectToGameViewModel;

		#region Unity methods

		private void Awake()
		{
			heroInput.onValueChanged.AddListener(HeroNameValueChanged);
			gameNameInput.onValueChanged.AddListener(GameNameValueChanged);
			toMainMenuButton.onClick.AddListener(OnToMainMenuButtonClick);
			updateButton.onClick.AddListener(OnUpdateButtonClick);
			createGameButton.onClick.AddListener(OnCreateGameButtonClick);
			connectToGameButton.onClick.AddListener(OnConnectToGameClickAsync);
		}

		private async void OnEnable()
		{
			if (_connectToGameViewModel is null) return;

			await _connectToGameViewModel.StopHub();

			SetInteractableToButtons();

			OnUpdateButtonClick();
		}

		#endregion

		#region Input fields values changed handlers

		private void HeroNameValueChanged(string value)
		{
			_connectToGameViewModel.OnHeroNameValueChanged(_heroIcon, _validSprite, _errorSprite, value);

			SetInteractableToButtons();
		}

		private void GameNameValueChanged(string value)
		{
			_connectToGameViewModel.OnLobbyNameValueChanged(_gameIcon, _validSprite, _errorSprite, value);

			SetInteractableToButtons();
		}

		#endregion

		#region Buttons handlers

		private void OnToMainMenuButtonClick()
		{
			PlayButtonClickSound();

			_connectToGameViewModel.ToMainMenu();
		}

		private void OnUpdateButtonClick()
		{
			PlayButtonClickSound();

			_connectToGameViewModel.SendUpdateLobbiesListRequest();
		}

		private void OnCreateGameButtonClick()
		{
			PlayButtonClickSound();

			//Sending CreateLobbyRequest
			_connectToGameViewModel.SendCreateLobbyRequest();
		}

		private async void OnConnectToGameClickAsync()
		{
			PlayButtonClickSound();

			await _connectToGameViewModel.SendConnectToLobbyRequest();
		}

		#endregion

		#region Commands

		public void OnLobbiesListUpdate()
		{
			LobbiesListController.Instance.UpdateLobbiesListDisplay(
				GameManager.Instance.ConnectToGameDataStore.Lobbies,
				lobbiesListItemPrefab,
				connectToGameButton);

			SetInteractableToButtons();
		}

		#endregion

		#region Ui controllers

		public void SetInteractableToButtons()
		{
			_connectToGameViewModel.SetInteractableToButtons(connectToGameButton, createGameButton);
		}

		#endregion

		protected override void OnBind(ConnectToGameViewModel connectToGameViewModel)
		{
			_connectToGameViewModel = connectToGameViewModel;
		}
	}
}