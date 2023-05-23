using Assets.Scripts.Components;
using Assets.Scripts.View;
using Assets.Scripts.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using View.Abstract;

namespace Assets.Scripts.Managers
{
	public class GameManager : ComponentPersistentSingleton<GameManager>
	{
		internal MainDataStore MainDataStore { get; private set; } = new MainDataStore();

		public static event Action<GameState> OnBeforeStateChanged;
		public static event Action<GameState> OnAfterStateChanged;

		public GameState State { get; private set; }

		void Start() => ChangeState(GameState.Starting);

		public void ChangeState(GameState newState)
		{
			OnBeforeStateChanged?.Invoke(newState);

			State = newState;
			switch (newState)
			{
				case GameState.Starting:
					HandleStarting();
					break;
				case GameState.LoadLoginRegisterScene:
					HandleLoadLoginRegisterScene();
					break;
				case GameState.Login:
					HandleLogin();
					break;
				case GameState.Register:
					HandleRegister();
					break;
				case GameState.LoadConnectToGameScene:
					HandleLoadConnectToGameScene();
					break;
				case GameState.ConnectToGame:
					HandleConnectToGame();
					break;
				case GameState.Lobby:
					HandleLobby();
					break;
				case GameState.LoadMainGameScene:
					HandleLoadMainGameScene();
					break;
				case GameState.MainGame:
					HandleMainGame();
					break;
				case GameState.MainGameMenu:
					HandleMainGameMenu();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
			}

			OnAfterStateChanged?.Invoke(newState);

			if (Debug.isDebugBuild)
				Debug.Log($"New state: {newState}");
		}

		private void HandleStarting()
		{
			ChangeState(GameState.LoadLoginRegisterScene);
		}

		private void HandleLoadLoginRegisterScene()
		{
			FPSView fpsView = GameObject.Find("cnvs_FPS")?.GetComponent<FPSView>();
			LoginView loginView = GameObject.Find("cnvs_login")?.GetComponent<LoginView>();
			RegisterView registerView = GameObject.Find("cnvs_register")?.GetComponent<RegisterView>();

			if (fpsView is null || loginView is null || registerView is null) throw new DataException();

			List<BaseScreen> screens = new List<BaseScreen>() { fpsView, loginView, registerView };

			UiManager.Instance.Init(screens);

			ChangeState(GameState.Login);
		}

		private void HandleLogin()
		{
			UiManager.Instance.BindAndShow(new FPSViewModel());
			UiManager.Instance.BindAndShow(new LoginViewModel());
			UiManager.Instance.Hide<RegisterViewModel>();
		}

		private void HandleRegister()
		{
			UiManager.Instance.BindAndShow(new FPSViewModel());
			UiManager.Instance.BindAndShow(new RegisterViewModel());
			UiManager.Instance.Hide<LoginViewModel>();
		}

		private void HandleLoadConnectToGameScene()
		{
			SceneManager.LoadScene(1);

			SceneManager.sceneLoaded += OnConnectToGameSceneLoaded;
		}

		private void OnConnectToGameSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (scene.buildIndex == 1) // Проверяем, что это нужная сцена
			{
				FPSView fpsView = GameObject.Find("cnvs_FPS")?.GetComponent<FPSView>();
				ConnectToGameView connectToGameView = GameObject.Find("cnvs_connectToGame")?.GetComponent<ConnectToGameView>();
				LobbyView lobbyView = GameObject.Find("cnvs_lobby")?.GetComponent<LobbyView>();

				if (fpsView is null || connectToGameView is null || lobbyView is null)
				{
					throw new DataException();
				}

				List<BaseScreen> screens = new List<BaseScreen>() { fpsView, connectToGameView, lobbyView };

				UiManager.Instance.Init(screens);

				SceneManager.sceneLoaded -= OnConnectToGameSceneLoaded;

				ChangeState(GameState.ConnectToGame);
			}
			else throw new DataException();

			
		}

		private void HandleConnectToGame()
		{
			UiManager.Instance.BindAndShow(new FPSViewModel());
			UiManager.Instance.BindAndShow(new ConnectToGameViewModel());
			UiManager.Instance.Hide<LobbyViewModel>();
		}

		private void HandleLobby()
		{
			UiManager.Instance.BindAndShow(new FPSViewModel());
			UiManager.Instance.BindAndShow(new LobbyViewModel());
			UiManager.Instance.Hide<ConnectToGameViewModel>();
		}

		private void HandleLoadMainGameScene()
		{
			SceneManager.LoadScene(2);
		}

		private void HandleMainGame()
		{

		}

		private void HandleMainGameMenu()
		{

		}
	}

	public enum GameState
	{
		Starting = 0,
		LoadLoginRegisterScene = 1,
		Login = 2,
		Register = 3,
		LoadConnectToGameScene = 4,
		ConnectToGame = 5,
		Lobby = 6,
		LoadMainGameScene = 7,
		MainGame = 8,
		MainGameMenu = 9,
	}
}


