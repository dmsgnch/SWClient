using Assets.Scripts.Components;
using Assets.Scripts.Managers;
using Components.Abstract;
using Components;
using LocalManagers.RegisterLoginRequests;
using SharedLibrary.Responses.Abstract;
using SharedLibrary.Responses;
using UnityEngine;
using ViewModels.Abstract;
using UnityEngine.UI;
using Assets.Scripts.View;
using Unity.VisualScripting;

namespace Assets.Scripts.ViewModels
{
	public class MainMenuViewModel : ViewModelBase
	{
		private GameObject ConfirmationPanel { get; set; }

		public void CloseApplication(MainMenuView mainMenuView, GameObject confirmPrefab)
		{
			if (!ConfirmationPanel.IsDestroyed() && ConfirmationPanel is not null) return;

			ConfirmationPanel = Object.Instantiate(confirmPrefab, mainMenuView.gameObject.transform);

			ConfirmationPanel.SetActive(true);
			ConfirmationPanel.transform.GetChild(0).GetComponent<Text>().text = "Are you sure you want to quit?";
			ConfirmationPanel.transform.GetChild(1).gameObject.
				GetComponent<Button>().onClick.AddListener(() =>
				{
					mainMenuView.PlayButtonClickSound();
					Application.Quit();
				});

			ConfirmationPanel.transform.GetChild(2).gameObject.
				GetComponent<Button>().onClick.AddListener(() =>
				{
					mainMenuView.PlayButtonClickSound();
					Object.Destroy(ConfirmationPanel);
				});
		}
	}
}
