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
		[SerializeField] private Button newGameButton;
		[SerializeField] private Button aboutButton;
		[SerializeField] private Button quitButton;

		[SerializeField] private GameObject confirmationPrefab;
        [SerializeField] private GameObject aboutPrefab;
		private GameObject _about;

        private MainMenuViewModel _mainMenuViewModel;

		#region Unity methods

		private void Awake()
		{
			newGameButton.onClick.AddListener(OnNewGameButtonClick);
			aboutButton.onClick.AddListener(OnAboutButtonClick);
			quitButton.onClick.AddListener(OnQuitGameButtonClick);
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				if (_about is not null)
				{
					Destroy(_about);
				}
				else
				{
					_mainMenuViewModel.CloseApplication(this, confirmationPrefab, gameObject);
				}
			}
		}

		#endregion

		#region Buttons handlers

		private void OnContinueButtonClick()
		{
			PlayButtonClickSound();

			Debug.Log("Continue");
		}

		private void OnLoadButtonClick()
		{
			PlayButtonClickSound();

			Debug.Log("Load");
		}

		private void OnNewGameButtonClick()
		{
			PlayButtonClickSound();

			GameManager.Instance.ChangeState(GameState.LoadConnectToGameScene);
		}

		private void OnSettingsButtonClick()
		{
			PlayButtonClickSound();

			Debug.Log("Settings");
		}

		private void OnAboutButtonClick()
		{
			PlayButtonClickSound();
			_about = Instantiate(aboutPrefab,gameObject.transform);
			Debug.Log("About");
		}

		private void OnQuitGameButtonClick()
		{
			PlayButtonClickSound();

			_mainMenuViewModel.CloseApplication(this, confirmationPrefab, gameObject);
		}
		
		#endregion

		protected override void OnBind(MainMenuViewModel mainMenuViewModel)
		{
			_mainMenuViewModel = mainMenuViewModel;
		}		
	}
}