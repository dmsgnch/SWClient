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

		[SerializeField] private UnityEngine.UI.Button quitButton;
		[SerializeField] private UnityEngine.UI.Button updateButton;
		[SerializeField] private UnityEngine.UI.Button connectToGameButton;
		[SerializeField] private UnityEngine.UI.Button createGameButton;
		[SerializeField] private GameObject lobbiesListItemPrefab;

		[SerializeField] protected UnityEngine.UI.Image _heroIcon;
		[SerializeField] protected UnityEngine.UI.Image _gameIcon;
		[SerializeField] protected Sprite _validSprite;
		[SerializeField] protected Sprite _errorSprite;

		private ConnectToGameViewModel _connectToGameViewModel;

		private void Awake()
		{
			heroInput.onValueChanged.AddListener(HeroNameValueChanged);
			gameNameInput.onValueChanged.AddListener(GameNameValueChanged);
			quitButton.onClick.AddListener(OnQuitButtonClick);
			updateButton.onClick.AddListener(OnUpdateButtonClick);
			createGameButton.onClick.AddListener(OnCreateGameButtonClick);
			connectToGameButton.onClick.AddListener(OnConnectToGameClickAsync);
		}

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

        private void OnQuitButtonClick()
		{
			_connectToGameViewModel.CloseApplication();
		}

		private void OnUpdateButtonClick()
		{
			_connectToGameViewModel.UpdateLobbiesList();			
		}

		public void OnLobbiesListUpdate()
		{
			LobbiesListController.Instance.UpdateLobbiesListDisplay(
				GameManager.Instance.ConnectToGameDataStore.Lobbies,
				lobbiesListItemPrefab,
				connectToGameButton);

			SetInteractableToButtons(); 
		}

		public void SetInteractableToButtons()
		{
            _connectToGameViewModel.SetInteractableToButtons(connectToGameButton, createGameButton);
        }

        private void OnCreateGameButtonClick()
		{
			//Sending CreateLobbyRequest
			_connectToGameViewModel.CreateLobby();
		}

		private async void OnConnectToGameClickAsync()
		{
			await _connectToGameViewModel.ConnectToLobby();
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				//TODO: Add confirm window

				_connectToGameViewModel.CloseApplication();
			}
		}

		private async void OnEnable()
		{
			if (_connectToGameViewModel is null) return;

			await _connectToGameViewModel.StopHub();

            SetInteractableToButtons();

            OnUpdateButtonClick();
		}

		protected override void OnBind(ConnectToGameViewModel connectToGameViewModel)
		{
			_connectToGameViewModel = connectToGameViewModel;
		}
	}
}