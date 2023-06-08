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

		public void ContinueGame()
		{
			Debug.Log("Continue");
			GameManager.Instance.ChangeState(GameState.MainGame);
		}

		public void LeaveTheGame(MenuView menuView, GameObject confirmPrefab)
		{
			var panel = Object.Instantiate(confirmPrefab, menuView.gameObject.transform);

			panel.SetActive(true);
			panel.transform.GetChild(0).GetComponent<Text>().text = "Are you sure you want to leave the game?";
			panel.transform.GetChild(1).gameObject.
				GetComponent<Button>().onClick.AddListener(() =>
				{
					NetworkingManager.Instance.StopHub().Wait();
					GameManager.Instance.ChangeState(GameState.LoadConnectToGameScene);
				});
			panel.transform.GetChild(1).gameObject.
				GetComponent<Button>().onClick.AddListener(() => menuView.PlayButtonClickSound());

			panel.transform.GetChild(2).gameObject.
				GetComponent<Button>().onClick.AddListener(() => Object.Destroy(panel));
			panel.transform.GetChild(2).gameObject.
				GetComponent<Button>().onClick.AddListener(() => menuView.PlayButtonClickSound());
		}

		public void CloseApplication(MenuView menuView, GameObject confirmPrefab)
		{
			var panel = Object.Instantiate(confirmPrefab, menuView.gameObject.transform);

			panel.SetActive(true);
			panel.transform.GetChild(0).GetComponent<Text>().text = "Are you sure you want to quit?";
			panel.transform.GetChild(1).gameObject.
				GetComponent<Button>().onClick.AddListener(() => Application.Quit());
			panel.transform.GetChild(1).gameObject.
				GetComponent<Button>().onClick.AddListener(() => menuView.PlayButtonClickSound());

			panel.transform.GetChild(2).gameObject.
				GetComponent<Button>().onClick.AddListener(() => Object.Destroy(panel));
			panel.transform.GetChild(2).gameObject.
				GetComponent<Button>().onClick.AddListener(() => menuView.PlayButtonClickSound());
		}

		#endregion
	}
}
