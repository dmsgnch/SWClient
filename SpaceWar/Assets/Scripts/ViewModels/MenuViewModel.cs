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

namespace Assets.Scripts.ViewModels
{
	public class MenuViewModel : ViewModelBase
	{
		public static bool GameIsPaused = false;
		public GameObject menuUI;
		
		public MenuViewModel() { }
		public void ContinueGame()
		{
			Debug.Log("Continue");
            Time.timeScale = 1f;
            GameIsPaused = false;
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
		public void QuitTheGame()
		{
			Debug.Log("Quit");
			Application.Quit();
		}
	}
}
