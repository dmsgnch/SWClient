using Assets.Scripts.Components;
using Assets.Scripts.View;
using Assets.Scripts.ViewModels;
using System;
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
		private UiManager uiManager;

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
			uiManager = FindObjectOfType<UiManager>();

			FPSView fpsView = GameObject.Find("cnvs_FPS").GetComponent<FPSView>();
			LoginView loginView = GameObject.Find("cnvs_login").GetComponent<LoginView>();
			RegisterView registerView = GameObject.Find("cnvs_register").GetComponent<RegisterView>();

			if (fpsView is null || loginView is null || registerView is null) throw new DataException();

			List<BaseScreen> screens = new List<BaseScreen>() { fpsView, loginView, registerView };

			uiManager.Init(screens);

			ChangeState(GameState.Login);
		}

		private void HandleLogin()
		{
			uiManager.BindAndShow(new FPSViewModel());
			uiManager.BindAndShow(new LoginViewModel());
			uiManager.Hide<RegisterViewModel>();
		}

		private void HandleRegister()
		{
			uiManager.BindAndShow(new FPSViewModel());
			uiManager.BindAndShow(new RegisterViewModel());
			uiManager.Hide<LoginViewModel>();
		}

		private void HandleLoadConnectToGameScene()
		{
			SceneManager.LoadScene(1);

			uiManager = FindObjectOfType<UiManager>();

			FPSView fpsView = GameObject.Find("cnvs_FPS").GetComponent<FPSView>();
			ConnectToGameView connectToGameView = GameObject.Find("cnvs_connectToGame").GetComponent<ConnectToGameView>();
			LobbyView lobbyView = GameObject.Find("cnvs_lobby").GetComponent<LobbyView>();

			if (fpsView is null || connectToGameView is null || lobbyView is null) throw new DataException();

			List<BaseScreen> screens = new List<BaseScreen>() { fpsView, connectToGameView, lobbyView };

			uiManager.Init(screens);
		}

		private void HandleConnectToGame()
		{
			uiManager.BindAndShow(new FPSViewModel());
			uiManager.BindAndShow(new ConnectToGameViewModel());
			uiManager.Hide<LobbyViewModel>();
		}

		private void HandleLobby()
		{
			uiManager.BindAndShow(new FPSViewModel());
			uiManager.BindAndShow(new LobbyViewModel());
			uiManager.Hide<ConnectToGameViewModel>();
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


