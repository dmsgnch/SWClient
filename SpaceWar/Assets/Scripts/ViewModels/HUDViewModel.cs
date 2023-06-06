using Assets.Scripts.Components.DataStores;
using Assets.Scripts.LocalManagers._2_MainGameScripts.RequestsAndResponses.Requests;
using Assets.Scripts.Managers;
using SharedLibrary.Models;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ViewModels.Abstract;
using Object = UnityEngine.Object;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SharedLibrary.Models.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Contracts.Hubs;
using System;
using SharedLibrary.Requests;

namespace Assets.Scripts.ViewModels
{
	public class HUDViewModel : ViewModelBase
	{
		public static GameObject CreateGetSessionRequestObject { get; set; }
		public static GameObject CreateGetHeroRequestObject { get; set; }

		private GameObject _resourcesInfoPanel;
		private GameObject _soldiersInfoPanel;
		private GameObject _researchShipInfoPanel;
		private GameObject _colonizeShipInfoPanel;
        private float time = 60f;
        private bool enableTimer = true;

        public void ReduceTurnPanelTime(GameObject turnPanel, float value)
        {
            if (enableTimer)
            {
                time -= value;
                if (time <= 0f)
                {
                    TurnPanelTimeOut(turnPanel);
                    enableTimer = false;
                }
                SetTurnPanelTime(turnPanel);
            }
        }
        public void SetTurnPanelTimer(GameObject turnPanel, float value)
        {
            time = value;
            enableTimer = true;
        }
        private void SetTurnPanelTime(GameObject turnPanel)
        {
            TMP_Text[] turnTexts = turnPanel.GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text tmpText in turnTexts)
            {
                if (tmpText.gameObject.name == "txt_leftTimeValue")
                {
                    tmpText.text = ((int)time).ToString();
                    break;
                }
            }
        }

        private void TurnPanelTimeOut(GameObject turnPanel)
        {
            TMP_Text[] turnTexts = turnPanel.GetComponentsInChildren<TMP_Text>();
			TMP_Text timeLeftText = turnTexts.FirstOrDefault(
				t => t.gameObject.name.Equals("txt_leftTimeValue"));
			timeLeftText.text = "OUT OF TIME";
            Debug.Log("OUT OF TIME");
			enableTimer = false;
        }
		public void ToMenu()
		{
			//Time.timeScale = 0f;
            GameManager.Instance.ChangeState(GameState.MainGameMenu);
        }

		public async void SendNextTurnRequest()
		{
			HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
			Guid lobbyId = GameManager.Instance.LobbyDataStore.LobbyId;

			await hubConnection.InvokeAsync(ServerHandlers.Session.NextTurn, new NextTurnRequest
			{
				HeroId = GameManager.Instance.HeroDataStore.HeroId,
				SessionId = GameManager.Instance.SessionDataStore.SessionId
			});
		}


		public void SetTurnButtonUnactiveStatus(GameObject turnPanel) {
			var buttonGO = turnPanel.transform.GetChild(0);
			var buttonImage = buttonGO.GetComponent<Image>();
			var buttonText = buttonGO.GetComponentInChildren<TMP_Text>();
            var button = buttonGO.GetComponent<Button>();
            UnityEngine.Color color;
			UnityEngine.ColorUtility.TryParseHtmlString("#CD393F", out color);
            buttonImage.color = color;
			buttonText.text = "Wait for other player";
			button.interactable = false;
		}

        public void SetTurnButtonActiveStatus(GameObject turnPanel)
        {
            var buttonGO = turnPanel.transform.GetChild(0);
            var buttonImage = buttonGO.GetComponent<Image>();
            var buttonText = buttonGO.GetComponentInChildren<TMP_Text>();
            var button = buttonGO.GetComponent<Button>();
            UnityEngine.Color color;
            UnityEngine.ColorUtility.TryParseHtmlString("#539F61", out color);
            buttonImage.color = color; ;
            buttonText.text = "Next Turn";
            button.interactable = true;
        }

        public void UpdateHeroDataPanelTexts(
			GameObject resourcesPanel, 
			GameObject soldiersPanel,
			GameObject researchShipsPanel, 
			GameObject colonizationShipsPanel)
		{
			resourcesPanel.GetComponentInChildren<TMP_Text>().text = $"{GameManager.Instance.HeroDataStore.Resourses}" +
				$"(+{GameManager.Instance.HeroDataStore.HeroMapView.Planets.Where(p => p.ResourceType.Equals(ResourceType.OnlyResources) && p.Status.Equals(PlanetStatus.Colonized)).Select(p =>p.ResourceCount).Sum()})";

			soldiersPanel.GetComponentInChildren<TMP_Text>().text = $"{GameManager.Instance.HeroDataStore.AvailableSoldiers}" +
				$"(+{(int)(GameManager.Instance.HeroDataStore.SoldiersLimit*0.2)})";

			researchShipsPanel.GetComponentInChildren<TMP_Text>().text = $"{GameManager.Instance.HeroDataStore.AvailableResearchShips}/" +
				$"{GameManager.Instance.HeroDataStore.ResearchShipLimit}";

            colonizationShipsPanel.GetComponentInChildren<TMP_Text>().text = $"{GameManager.Instance.HeroDataStore.AvailableColonizationShips}/" +
            $"{GameManager.Instance.HeroDataStore.ColonizationShipLimit}";

		}

		public void UpdateSessionDataPanelTexts(GameObject turnPanel)
		{
			TMP_Text[] turnTexts = turnPanel.GetComponentsInChildren<TMP_Text>();
			foreach (TMP_Text tmpText in turnTexts)
			{
				switch (tmpText.gameObject.name)
				{
					case "txt_currentTurnValue":
						int turnNumberView = GameManager.Instance.SessionDataStore.TurnNumber + 1;
						tmpText.text = turnNumberView.ToString();
						break;
				}
			}
		}

		public void SetHeroNewValues(Hero hero)
		{
            GameManager.Instance.HeroDataStore.Name = hero.Name;
			GameManager.Instance.HeroDataStore.Resourses = hero.Resourses;
            GameManager.Instance.HeroDataStore.SoldiersLimit = hero.SoldiersLimit;
            GameManager.Instance.HeroDataStore.AvailableSoldiers = hero.AvailableSoldiers;
            GameManager.Instance.HeroDataStore.ResearchShipLimit = hero.ResearchShipLimit;
            GameManager.Instance.HeroDataStore.AvailableResearchShips = hero.AvailableResearchShips;
			GameManager.Instance.HeroDataStore.ColonizationShipLimit = hero.ColonizationShipLimit;       
            GameManager.Instance.HeroDataStore.AvailableColonizationShips = hero.AvailableColonizationShips;
		}

        public async void ShowChangePanel(string message, GameObject changePanelPrefab, GameObject parentPanel)
        {
            GameObject changePanelGO = null;
            string panelName = parentPanel.name + "_message";
			for (int i = 1; i <= 10; i++)
			{
				if (!GameObject.Find(panelName + $"_{i}"))
				{
					changePanelGO = GameObject.Instantiate(changePanelPrefab, GameObject.Find("cnvs_HUD").transform);
					panelName += $"_{i}";
					changePanelGO.name = panelName;
					changePanelGO.transform.position = parentPanel.transform.position + Vector3.down * 45 * i;
                    var text = changePanelGO.GetComponentInChildren<TMP_Text>();
                    text.text = message;

                    if (text.text.Contains('-')) text.color = UnityEngine.Color.red;
                    else text.color = UnityEngine.Color.green;

                    await Task.Delay(3000);
                    GameObject.Destroy(changePanelGO);
					return;
                }
			}
        }

        public void GetSessionRequestCreate()
		{
			CreateGetSessionRequestObject = new GameObject("GetSessionRequest");

			var createGetSessionRequest = CreateGetSessionRequestObject.AddComponent<GetSessionRequest>();

			createGetSessionRequest.CreateRequest();
		}

		public void GetHeroRequestCreate()
		{
			CreateGetHeroRequestObject = new GameObject("GetHeroRequest");

			var createGetHeroRequest = CreateGetHeroRequestObject.AddComponent<GetHeroRequest>();

			createGetHeroRequest.CreateRequest();
        }

        public void UpdatePlayerList(GameObject playerList,GameObject playerName_Prefab)
        {
            while (playerList.transform.childCount > 0)
            {
                GameObject.DestroyImmediate(playerList.transform.GetChild(0).gameObject);
            }
            var sessionDataStore = GameManager.Instance.SessionDataStore;

			Vector3 panelPosition = Vector3.zero;
            foreach (var player in sessionDataStore.PanelHeroForms)
            {
                GameObject playerPanel = GameObject.Instantiate(playerName_Prefab, playerList.transform);
				if (player.HeroId.Equals(GameManager.Instance.SessionDataStore.CurrentHeroTurnId))
				{
					playerPanel.GetComponent<Image>().color = 
						new UnityEngine.Color((float)163/256, (float)8/256, (float)166/256);
                }
                if(!panelPosition.Equals(Vector3.zero))
					playerPanel.transform.position = playerPanel.transform.position + panelPosition;
                panelPosition = panelPosition + Vector3.down * 80;
                playerPanel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = player.HeroName;
            }
        }

        #region HUD panels
        public void CreateResourcePanel(GameObject resourcesInfoPanelPrefab, Transform parent)
		{
			if (_resourcesInfoPanel is null)
			{
				_resourcesInfoPanel = MonoBehaviour.Instantiate(resourcesInfoPanelPrefab, parent);
			}

			TMP_Text[] tmpTextComponents = _resourcesInfoPanel.GetComponentsInChildren<TMP_Text>();

			foreach (TMP_Text tmpText in tmpTextComponents)
			{
				if (tmpText.gameObject.name == "txt_resourcesValue")
					tmpText.text = GameManager.Instance.HeroDataStore.Resourses.ToString();
			}
		}

		public void CreateSoldiersPanel(GameObject soldiersInfoPanelPrefab, Transform parent)
		{
			if (_soldiersInfoPanel is null)
			{
				_soldiersInfoPanel = MonoBehaviour.Instantiate(soldiersInfoPanelPrefab, parent);
			}

			TMP_Text[] tmpTextComponents = _soldiersInfoPanel.GetComponentsInChildren<TMP_Text>();

			foreach (TMP_Text tmpText in tmpTextComponents)
			{
				switch (tmpText.gameObject.name)
				{
					case "txt_totalSoldiersValue":
						tmpText.text = GameManager.Instance.HeroDataStore.SoldiersLimit.ToString();
						break;
					case "txt_usedSoldiersValue":
						tmpText.text = GameManager.Instance.HeroDataStore.AvailableSoldiers.ToString();
						break;
				}
			}
		}

		public void CreateResearchShipPanel(GameObject researchShipInfoPanelPrefab, Transform parent)
		{
			if (_researchShipInfoPanel is null)
			{
				_researchShipInfoPanel = MonoBehaviour.Instantiate(researchShipInfoPanelPrefab, parent);
			}

			TMP_Text[] tmpTextComponents = _researchShipInfoPanel.GetComponentsInChildren<TMP_Text>();

			foreach (TMP_Text tmpText in tmpTextComponents)
			{
				switch (tmpText.gameObject.name)
				{
					case "txt_totalShipsValue":
						tmpText.text = GameManager.Instance.HeroDataStore.ResearchShipLimit.ToString();
						break;
					case "txt_usedShipsValue":
						tmpText.text = GameManager.Instance.HeroDataStore.AvailableResearchShips.ToString();
						break;
				}
			}
		}

		public void CreateColonizeShipPanel(GameObject colonizeShipInfoPanelPrefab, Transform parent)
		{
			if (_colonizeShipInfoPanel is null)
			{
				_colonizeShipInfoPanel = MonoBehaviour.Instantiate(colonizeShipInfoPanelPrefab, parent);
			}

			TMP_Text[] tmpTextComponents = _colonizeShipInfoPanel.GetComponentsInChildren<TMP_Text>();

			foreach (TMP_Text tmpText in tmpTextComponents)
			{
				switch (tmpText.gameObject.name)
				{
					case "txt_totalShipsValue":
						tmpText.text = GameManager.Instance.HeroDataStore.ColonizationShipLimit.ToString();
						break;
					case "txt_usedShipsValue":
						tmpText.text = GameManager.Instance.HeroDataStore.AvailableColonizationShips.ToString();
						break;
				}
			}
		}

		public void DeleteResourcePanel()
		{
			Object.Destroy(_resourcesInfoPanel);
			_resourcesInfoPanel = null;
		}

		public void DeleteSoldiersPanel()
		{
			Object.Destroy(_soldiersInfoPanel);
			_soldiersInfoPanel = null;
		}

		public void DeleteResearchShipPanel()
		{
			Object.Destroy(_researchShipInfoPanel);
			_researchShipInfoPanel = null;
		}

		public void DeleteColonizeShipPanel()
		{
			Object.Destroy(_colonizeShipInfoPanel);
			_colonizeShipInfoPanel = null;
		}
        #endregion
    }
}
