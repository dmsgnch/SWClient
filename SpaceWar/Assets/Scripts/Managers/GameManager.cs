using Assets.Scripts.Components;
using Assets.Scripts.Components.DataStores;
using Assets.Scripts.View;
using Assets.Scripts.ViewModels;
using Components;
using Scripts.RegisterLoginScripts;
using LocalManagers.RegisterLoginRequests;
using OpenCover.Framework.Model;
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
		[SerializeField] private GameObject loadManagerPrefab;

		internal MainDataStore MainDataStore { get; private set; } = new MainDataStore();
		internal ConnectToGameDataStore ConnectToGameDataStore { get; private set; } = new ConnectToGameDataStore();
		internal LobbyDataStore LobbyDataStore { get; private set; } = new LobbyDataStore();
		internal SessionDataStore SessionDataStore { get; private set; } = new SessionDataStore();
		internal HeroDataStore HeroDataStore { get; private set; } = new HeroDataStore();
		internal BattleDataStore BattleDataStore { get; private set; } = new BattleDataStore();

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

			if (fpsView is null || loginView is null || registerView is null)
			{
				throw new DataException();
			}

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
			GameObject loadingManagerObject = Instantiate(loadManagerPrefab);

			LoadingManager loadingManager = loadingManagerObject.GetComponent<LoadingManager>();

			SceneManager.sceneLoaded += OnConnectToGameSceneLoaded;

			loadingManager.LoadScene(1);
		}

        private void OnConnectToGameSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if(scene.buildIndex != 1) throw new DataException();

            GameObject[] canvases = Resources.FindObjectsOfTypeAll(typeof(Canvas))
                .Select(o => (o as Canvas).gameObject).ToArray();

			if (canvases is null || canvases.Length.Equals(0))
			{
				throw new DataException();
			}

			FPSView fpsView = canvases.FirstOrDefault(c => c.name == "cnvs_FPS")?
				.GetComponent<FPSView>();

			ConnectToGameView connectToGameView = canvases.FirstOrDefault(c => c.name == "cnvs_connectToGame")?
				.GetComponent<ConnectToGameView>();

			LobbyView lobbyView = canvases.FirstOrDefault(c => c.name == "cnvs_lobby")?
				.GetComponent<LobbyView>();

			if (fpsView is null || connectToGameView is null || lobbyView is null)
			{
				throw new DataException();
			}

			List<BaseScreen> screens = new List<BaseScreen>() { fpsView, connectToGameView, lobbyView };

			UiManager.Instance.Init(screens);

			SceneManager.sceneLoaded -= OnConnectToGameSceneLoaded;

			ChangeState(GameState.ConnectToGame);		
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
			UiManager.Instance.Hide<ConnectToGameViewModel>();
			UiManager.Instance.BindAndShow(new LobbyViewModel());
		}

		private void HandleLoadMainGameScene()
		{
			GameObject loadingManagerObject = Instantiate(loadManagerPrefab);

			LoadingManager loadingManager = loadingManagerObject.GetComponent<LoadingManager>();

			SceneManager.sceneLoaded += OnMainGameSceneLoaded;

			loadingManager.LoadScene(2);
		}

		private async void OnMainGameSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (scene.buildIndex != 2) throw new DataException("Index is not match");

			FPSView fpsView = GameObject.Find("cnvs_FPS")?.GetComponent<FPSView>(); 

            MainGameCameraView mainGameCameraView = GameObject.Find("Look_Camera")?.GetComponent<MainGameCameraView>();

			HUDView hudView = GameObject.Find("cnvs_HUD")?.GetComponent<HUDView>();

            PlanetsView planetsView = GameObject.Find("cnvs_mainGame")?.GetComponent<PlanetsView>();

			MenuView menuView = GameObject.Find("cnvs_menu")?.GetComponent<MenuView>();

            if (fpsView is null || hudView is null || planetsView is null || mainGameCameraView is null || menuView is null)
			{
				if(fpsView is null)
				{
					InformationPanelController.Instance.CreateMessage(InformationPanelController.MessageType.ERROR, "fpsView was not found");
				}
				if (hudView is null)
				{
					InformationPanelController.Instance.CreateMessage(InformationPanelController.MessageType.ERROR, "hudView was not found");
				}
				if (planetsView is null)
				{
					InformationPanelController.Instance.CreateMessage(InformationPanelController.MessageType.ERROR, "planetsView was not found");
				}
				if (mainGameCameraView is null)
				{
					InformationPanelController.Instance.CreateMessage(InformationPanelController.MessageType.ERROR, "mainGameCameraView was not found");
				}

				throw new DataException("Views were not found");
			}

			List<BaseScreen> screens = new List<BaseScreen>() 
			{
				fpsView, 
				hudView, 
				mainGameCameraView, 
				planetsView, 
				menuView
			};

			UiManager.Instance.Init(screens);

			SceneManager.sceneLoaded -= OnMainGameSceneLoaded;

			await NetworkingManager.Instance.StartHub("session");

			ChangeState(GameState.MainGame);
		}

		private void HandleMainGame()
		{
			UiManager.Instance.BindAndShow(new HUDViewModel());
			UiManager.Instance.BindAndShow(new FPSViewModel());
			UiManager.Instance.BindAndShow(new MainGameCameraViewModel());
            UiManager.Instance.BindAndShow(new PlanetsViewModel());
			UiManager.Instance.Hide<MenuViewModel>();
        }

		private void HandleMainGameMenu()
		{
			UiManager.Instance.BindAndShow(new MenuViewModel());
            UiManager.Instance.BindAndShow(new HUDViewModel());
            UiManager.Instance.BindAndShow(new FPSViewModel());
            UiManager.Instance.BindAndShow(new MainGameCameraViewModel());
            UiManager.Instance.BindAndShow(new PlanetsViewModel());
            //UiManager.Instance.Hide<HUDViewModel>();
            //UiManager.Instance.Hide<FPSViewModel>();
           // UiManager.Instance.BindAndShow(new MainGameCameraViewModel());
          // UiManager.Instance.Hide<PlanetsViewModel>();

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


