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

		public void SaveGame()
		{
			Debug.Log("Save Game");
		}

		public void LoadGame()
		{
			Debug.Log("Load Game");
		}

		public void Settings()
		{
			Debug.Log("Settings");
		}

		public async Task LeaveTheGame()
		{
			Debug.Log("LeaveTheGame");
			await NetworkingManager.Instance.StopHub();
			GameManager.Instance.ChangeState(GameState.LoadConnectToGameScene);
		}

		public void CloseApplication(MenuView menuView, GameObject confirmPrefab, GameObject parent)
		{
			var panel = MonoBehaviour.Instantiate(confirmPrefab, parent.transform);
			//panel.transform.SetParent(parent.transform);
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
