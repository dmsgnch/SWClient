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
using LocalManagers.ConnectToGame.ValueChangedHandlers;

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
		[SerializeField] private UnityEngine.UI.Button CreateGameButton;
		[SerializeField] private GameObject lobbiesListItemPrefab;

		private ConnectToGameViewModel _connectToGameViewModel;

		private void Awake()
		{
			heroInput.onValueChanged.AddListener(HeroNameValueChanged);
			gameNameInput.onValueChanged.AddListener(GameNameValueChanged);
			quitButton.onClick.AddListener(OnQuitButtonClick);
			updateButton.onClick.AddListener(OnUpdateButtonClick);
			//connectToGameButton.onClick.AddListener(_connectToGameViewModel.OnToRegisterButtonClick);
			//CreateGameButton.onClick.AddListener(_connectToGameViewModel.OnToRegisterButtonClick);
		}

		private void HeroNameValueChanged(string value)
		{
			HeroNameChangedHandler.Instance.OnValueChanged(value);

			_connectToGameViewModel.SetInteractableToButtons(connectToGameButton, CreateGameButton);
		}

		private void GameNameValueChanged(string value)
		{
			LobbyNameValueChangedHandler.Instance.OnValueChanged(value);

			_connectToGameViewModel.SetInteractableToButtons(connectToGameButton, CreateGameButton);
		}

		private void OnQuitButtonClick()
		{
			_connectToGameViewModel.CloseApplication();
		}

		private void OnUpdateButtonClick()
		{
			_connectToGameViewModel.UpdateLobbiesList(lobbiesListItemPrefab, connectToGameButton);

			_connectToGameViewModel.SetInteractableToButtons(connectToGameButton, CreateGameButton);
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				//TODO: Add confirm window			 

				_connectToGameViewModel.CloseApplication();
			}
		}

		protected override void OnBind(ConnectToGameViewModel connectToGameViewModel)
		{
			_connectToGameViewModel = connectToGameViewModel;

			connectToGameButton.interactable = false;
			CreateGameButton.interactable = false;

			_connectToGameViewModel.UpdateLobbiesList(lobbiesListItemPrefab, connectToGameButton);
		}
	}
}