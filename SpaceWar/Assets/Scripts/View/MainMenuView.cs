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
using Assets.Scripts.Managers;

namespace Assets.Scripts.View
{
	public class MainMenuView : AbstractScreen<MainMenuViewModel>
	{
		[SerializeField] private Button continueButton;
		[SerializeField] private Button loadButton;
		[SerializeField] private Button newGameButton;
		[SerializeField] private Button settingsButton;
		[SerializeField] private Button aboutButton;
		[SerializeField] private Button quitButton;

		[SerializeField] private GameObject confirmationPrefab;

		private MainMenuViewModel _mainMenuViewModel;

		private void Awake()
		{
			continueButton.onClick.AddListener(OnContinueButtonClick);
			loadButton.onClick.AddListener(OnLoadButtonClick);
			newGameButton.onClick.AddListener(OnNewGameButtonClick);
			settingsButton.onClick.AddListener(OnSettingsButtonClick);
			aboutButton.onClick.AddListener(OnAboutButtonClick);
			quitButton.onClick.AddListener(OnQuitGameButtonClick);
		}

		private void OnContinueButtonClick()
		{
			Debug.Log("Continue");
		}

		private void OnLoadButtonClick()
		{
			Debug.Log("Load");
		}

		private void OnNewGameButtonClick()
		{
			GameManager.Instance.ChangeState(GameState.LoadConnectToGameScene);
		}

		private void OnSettingsButtonClick()
		{
			Debug.Log("Settings");
		}

		private void OnAboutButtonClick()
		{
			Debug.Log("About");
		}

		private void OnQuitGameButtonClick()
		{
			_mainMenuViewModel.CloseApplication(confirmationPrefab, gameObject);
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				_mainMenuViewModel.CloseApplication(confirmationPrefab, gameObject);
			}
		}

		protected override void OnBind(MainMenuViewModel mainMenuViewModel)
		{
			_mainMenuViewModel = mainMenuViewModel;
		}		
	}
}