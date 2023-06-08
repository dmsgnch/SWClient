using Assets.Scripts.Components;
using Assets.Scripts.Managers;
using Components.Abstract;
using Components;
using LocalManagers.RegisterLoginRequests;
using SharedLibrary.Responses.Abstract;
using SharedLibrary.Responses;
using UnityEngine;
using ViewModels.Abstract;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using Scripts.RegisterLoginScripts;
using System.Threading.Tasks;
using UnityEngine.UI;
using Assets.Scripts.View;

namespace Assets.Scripts.ViewModels
{
	public class MenuViewModel : ViewModelBase
	{
		#region Buttons handlers

		private GameObject ConfirmationPanel { get; set; }

		public void ContinueGame()
		{
			Debug.Log("Continue");
			GameManager.Instance.ChangeState(GameState.MainGame);
		}

		public void LeaveTheGame(MenuView menuView, GameObject confirmPrefab)
		{
			if (!ConfirmationPanel.IsDestroyed() && ConfirmationPanel is not null) return;

			ConfirmationPanel = Object.Instantiate(confirmPrefab, menuView.gameObject.transform);

			ConfirmationPanel.SetActive(true);
			ConfirmationPanel.transform.GetChild(0).GetComponent<Text>().text = "Are you sure you want to leave the game?";
			ConfirmationPanel.transform.GetChild(1).gameObject.
				GetComponent<Button>().onClick.AddListener(() =>
				{
					menuView.PlayButtonClickSound();

					NetworkingManager.Instance.StopHub().Wait();
					GameManager.Instance.ChangeState(GameState.LoadConnectToGameScene);
				});

			ConfirmationPanel.transform.GetChild(2).gameObject.
				GetComponent<Button>().onClick.AddListener(() =>
				{
					menuView.PlayButtonClickSound();
					Object.Destroy(ConfirmationPanel);
				});
		}

		public void CloseApplication(MenuView menuView, GameObject confirmPrefab)
		{
			if (!ConfirmationPanel.IsDestroyed() && ConfirmationPanel is not null) return;

			ConfirmationPanel = Object.Instantiate(confirmPrefab, menuView.gameObject.transform);

			ConfirmationPanel.SetActive(true);
			ConfirmationPanel.transform.GetChild(0).GetComponent<Text>().text = "Are you sure you want to quit?";
			ConfirmationPanel.transform.GetChild(1).gameObject.
				GetComponent<Button>().onClick.AddListener(() =>
				{
					menuView.PlayButtonClickSound();
					Application.Quit();
				});

			ConfirmationPanel.transform.GetChild(2).gameObject.
				GetComponent<Button>().onClick.AddListener(() =>
				{
					menuView.PlayButtonClickSound();
					Object.Destroy(ConfirmationPanel);
				});
		}

		#endregion
	}
}
