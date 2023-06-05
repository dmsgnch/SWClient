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

namespace Assets.Scripts.ViewModels
{
	public class MenuViewModel : ViewModelBase
	{	
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

        public async Task QuitApplication()
		{
			Debug.Log("Quit");
            await NetworkingManager.Instance.StopHub();
            Application.Quit();
		}
	}
}
