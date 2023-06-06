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
		[SerializeField] private AudioClip[] Audios;

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
				case GameState.LoadMainMenuScene:
					HandleLoadScene(1);
					break;
				case GameState.MainMenu:
					HandleMainMenu();
					break;
				case GameState.LoadConnectToGameScene:
					HandleLoadScene(2);
					break;
				case GameState.ConnectToGame:
					HandleConnectToGame();
					break;
				case GameState.Lobby:
					HandleLobby();
					break;
				case GameState.LoadMainGameScene:
					HandleLoadScene(3);
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
			SceneManager.sceneLoaded += OnSceneLoaded;
			ChangeState(GameState.LoadLoginRegisterScene);
		}


		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			switch (scene.buildIndex)
			{
				case 0:
					break;
				case 1:
					OnMainMenuSceneLoaded(scene, mode);
					break;
				case 2:
					OnConnectToGameSceneLoaded(scene, mode);
					break;
				case 3:
					OnMainGameSceneLoaded(scene, mode);
					break;
				default:
					throw new ArgumentException("scene build index out of range!");
			}
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

			PlayMusic(0);

			ChangeState(GameState.Login);
		}

		private void PlayMusic(byte numberOfSoundtreck)
		{
			if (Audios.Length <= numberOfSoundtreck) throw new ArgumentException();

			AudioSystem audioSystem = GetSceneComponent<AudioSystem>();

			audioSystem.PlayMusic(Audios[numberOfSoundtreck]);
		}

		private T GetSceneComponent<T>() where T : Component
		{
			return FindFirstObjectByType<T>() ?? throw new Exception();
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

		private void HandleLoadScene(int sceneNumber)
		{
			GameObject loadingManagerObject = Instantiate(loadManagerPrefab);

			LoadingManager loadingManager = loadingManagerObject.GetComponent<LoadingManager>();

			loadingManager.LoadScene(sceneNumber);
		}

		private void OnMainMenuSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (scene.buildIndex != 1) throw new DataException();

			GameObject[] canvases = Resources.FindObjectsOfTypeAll(typeof(Canvas))
				.Select(o => (o as Canvas).gameObject).ToArray();

			if (canvases is null || canvases.Length.Equals(0))
			{
				throw new DataException();
			}

			FPSView fpsView = canvases.FirstOrDefault(c => c.name == "cnvs_FPS")?
				.GetComponent<FPSView>() ?? throw new DataException();

			MainMenuView mainMenuView = canvases.FirstOrDefault(c => c.name == "cnvs_menu")?
				.GetComponent<MainMenuView>();

			if (fpsView is null || mainMenuView is null)
			{
				throw new DataException();
			}

			List<BaseScreen> screens = new List<BaseScreen>() { fpsView, mainMenuView };

			UiManager.Instance.Init(screens);

			SceneManager.sceneLoaded -= OnMainMenuSceneLoaded;

			ChangeState(GameState.MainMenu);
		}

		private void HandleMainMenu()
		{
			UiManager.Instance.BindAndShow(new FPSViewModel());
			UiManager.Instance.BindAndShow(new MainMenuViewModel());
		}

		private void OnConnectToGameSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != 2) throw new DataException();

            GameObject[] canvases = Resources.FindObjectsOfTypeAll(typeof(Canvas))
                .Select(o => (o as Canvas).gameObject).ToArray();

            if (canvases is null || canvases.Length.Equals(0))
            {
                throw new DataException();
            }

            FPSView fpsView = canvases.FirstOrDefault(c => c.name == "cnvs_FPS")?
                .GetComponent<FPSView>() ?? throw new DataException();

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

		private async void OnMainGameSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != 3) throw new DataException("Index is not match");

            FPSView fpsView = GameObject.Find("cnvs_FPS")?.GetComponent<FPSView>();

            MainGameCameraView mainGameCameraView = GameObject.Find("Look_Camera")?.GetComponent<MainGameCameraView>();

            HUDView hudView = GameObject.Find("cnvs_HUD")?.GetComponent<HUDView>();

            PlanetsView planetsView = GameObject.Find("cnvs_mainGame")?.GetComponent<PlanetsView>();

            MenuView menuView = GameObject.Find("cnvs_menu")?.GetComponent<MenuView>();

            if (fpsView is null || hudView is null || planetsView is null
                || mainGameCameraView is null || menuView is null)
            {
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

			PlayMusic(1);

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
			UiManager.Instance.BindAndShow(new MainGameCameraViewModel());
			UiManager.Instance.Hide<HUDViewModel>();
			UiManager.Instance.Hide<FPSViewModel>();
			UiManager.Instance.Hide<PlanetsViewModel>();
		}
	}

	public enum GameState
	{
		Starting = 0,
		LoadLoginRegisterScene = 1,
		Login = 2,
		Register = 3,
		LoadMainMenuScene = 4,
		MainMenu = 5,
		LoadConnectToGameScene = 6,
		ConnectToGame = 7,
		Lobby = 8,
		LoadMainGameScene = 9,
		MainGame = 10,
		MainGameMenu = 11,
	}
}