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
using ColorUtility = UnityEngine.ColorUtility;
using Color = UnityEngine.Color;

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

		#region Timer

		private float timer = 0f;

		public void SetTimerNewValue(float value)
		{
			timer = value;
		}

		public void ReduceTimerValue(GameObject turnPanel)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;

                if (timer <= 0f)
                {
                    PostTimerTimeOutAction(turnPanel);
                }

                SetTimerValueOnHUD(turnPanel);
            }
        }

        private void SetTimerValueOnHUD(GameObject turnPanel, string value = null)
		{
            TMP_Text timerText = FindChildRecursive(turnPanel.transform, "txt_leftTimeValue").GetComponent<TMP_Text>();

			timerText.text = value ?? ((int)timer).ToString();   
        }

		public static Transform FindChildRecursive(Transform parent, string name)
		{
			Transform result = parent.Find(name);
			if (result != null)
				return result;

			foreach (Transform child in parent)
			{
				result = FindChildRecursive(child, name);
				if (result != null)
					return result;
			}

			return null;
		}

		private void PostTimerTimeOutAction(GameObject turnPanel)
        {
            TMP_Text timeLeftText = turnPanel.GetComponentsInChildren<TMP_Text>()?.FirstOrDefault(
				t => t.gameObject.name.Equals("txt_leftTimeValue"));

			timeLeftText.text = "OUT OF TIME";
			if (Debug.isDebugBuild) Debug.Log("OUT OF TIME");
        }

		#endregion

		#region Commands

		public void ToMenu()
		{
            GameManager.Instance.ChangeState(GameState.MainGameMenu);
        }

		#endregion

		#region Requests

		#region SignalR

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

		#endregion

		#region REST requests

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

		#endregion

		#endregion

		#region HUD panels values updators

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

		#endregion

		#region DataStores values setters

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

		#endregion

		#region Ui controllers

		public async void ShowChangePanel(string message, GameObject changePanelPrefab, GameObject parentPanel)
        {
            GameObject changePanelGO = null;

            string panelName = parentPanel.name + "_message";

			for (int i = 1; i <= 2; i++)
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

		#endregion

		#region Players list panel

		private static Color heroTurnColor = new Color((float)163 / 256, (float)8 / 256, (float)166 / 256);

		public void UpdatePlayersInHUDPanel(GameObject playerList, GameObject playerName_Prefab)
        {
			DestroyChildrenImmediately(playerList);

			var sessionDataStore = GameManager.Instance.SessionDataStore;

			Vector3 panelPosition = Vector3.zero;

            foreach (var hero in sessionDataStore.PanelHeroForms)
            {
                GameObject playerPanel = Object.Instantiate(playerName_Prefab, playerList.transform);

				if (hero.HeroId.Equals(GameManager.Instance.SessionDataStore.CurrentHeroTurnId))
				{
					playerPanel.GetComponent<Image>().color = heroTurnColor;
				}

				playerPanel.transform.position += panelPosition;
                panelPosition += Vector3.down * 80;

                playerPanel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = hero.HeroName;
            }
        }

		private void DestroyChildrenImmediately(GameObject playerList)
		{
			while (playerList.transform.childCount > 0)
			{
				Object.DestroyImmediate(playerList.transform.GetChild(0).gameObject);
			}
		}

		#endregion

		#region NextTurn panel

		public void SetNextTurnPanelValues(GameObject turnPanel)
		{
			if(GameManager.Instance.SessionDataStore.CurrentHeroTurnId.Equals(GameManager.Instance.HeroDataStore.HeroId))
			{
				SetTurnButtonInteractableStatus(turnPanel);

				SetTimerNewValue(GameManager.Instance.SessionDataStore.TurnTimeLimit);
			}
			else
			{
				SetTurnButtonUninteractableStatus(turnPanel);

				SetTimerValueOnHUD(turnPanel, "---");
			}
		}

		public void SetTurnButtonUninteractableStatus(GameObject turnPanel)
		{
			var buttonGO = turnPanel.transform.GetChild(0);

			ColorUtility.TryParseHtmlString("#CD393F", out UnityEngine.Color color);

			buttonGO.GetComponentInChildren<TMP_Text>().text = "Wait for other player";
			buttonGO.GetComponent<Button>().interactable = false;
			buttonGO.GetComponent<Image>().color = color;
		}

		public void SetTurnButtonInteractableStatus(GameObject turnPanel)
		{
			var buttonGO = turnPanel.transform.GetChild(0);

			ColorUtility.TryParseHtmlString("#539F61", out UnityEngine.Color color);

			buttonGO.GetComponent<Image>().color = color;
			buttonGO.GetComponentInChildren<TMP_Text>().text = "Next Turn";
			buttonGO.GetComponent<Button>().interactable = true;
		}

		#endregion

		#region HUD panels

		#region Creating

		private void InstantiateResourceInfoPanel(ref GameObject panelStore, 
			GameObject infoPanelPrefab, Transform parent)
		{
			if (panelStore is null)
			{
				panelStore = Object.Instantiate(infoPanelPrefab, parent);
			}
		}

		private TMP_Text GetTextComponentByChildObjectName(Transform panelStore, string objectName)
		{
            return panelStore.transform.Find(objectName)?.gameObject.GetComponent<TMP_Text>() 
				?? throw new NullReferenceException();
		}

		public void CreateResourcePanel(GameObject resourcesInfoPanelPrefab, Transform parent)
		{
			InstantiateResourceInfoPanel(ref _resourcesInfoPanel, resourcesInfoPanelPrefab, parent);

			GetTextComponentByChildObjectName(_resourcesInfoPanel.transform.GetChild(0), 
				"txt_resourcesValue").text = 
				GameManager.Instance.HeroDataStore.Resourses.ToString();
		}

		public void CreateSoldiersPanel(GameObject soldiersInfoPanelPrefab, Transform parent)
		{
			InstantiateResourceInfoPanel(ref _soldiersInfoPanel, soldiersInfoPanelPrefab, parent);

			GetTextComponentByChildObjectName(_soldiersInfoPanel.transform.GetChild(0), 
				"txt_totalSoldiersValue").text =
				GameManager.Instance.HeroDataStore.SoldiersLimit.ToString();

			GetTextComponentByChildObjectName(_soldiersInfoPanel.transform.GetChild(1), 
				"txt_usedSoldiersValue").text =
				GameManager.Instance.HeroDataStore.AvailableSoldiers.ToString();
		}

		public void CreateResearchShipPanel(GameObject researchShipInfoPanelPrefab, Transform parent)
		{
			InstantiateResourceInfoPanel(ref _researchShipInfoPanel, researchShipInfoPanelPrefab, parent);

			GetTextComponentByChildObjectName(_researchShipInfoPanel.transform.GetChild(0), 
				"txt_totalShipsValue").text =
				GameManager.Instance.HeroDataStore.ResearchShipLimit.ToString();

			GetTextComponentByChildObjectName(_researchShipInfoPanel.transform.GetChild(1), 
				"txt_usedShipsValue").text =
				GameManager.Instance.HeroDataStore.AvailableResearchShips.ToString();
		}

		public void CreateColonizeShipPanel(GameObject colonizeShipInfoPanelPrefab, Transform parent)
		{
			InstantiateResourceInfoPanel(ref _colonizeShipInfoPanel, colonizeShipInfoPanelPrefab, parent);

			GetTextComponentByChildObjectName(_colonizeShipInfoPanel.transform.GetChild(0), 
				"txt_totalShipsValue").text = 
				GameManager.Instance.HeroDataStore.ColonizationShipLimit.ToString();
            GetTextComponentByChildObjectName(_colonizeShipInfoPanel.transform.GetChild(1), 
				"txt_usedShipsValue").text = 
				GameManager.Instance.HeroDataStore.AvailableColonizationShips.ToString(); 
		}

		#endregion

		#region Deleting

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

		#endregion
	}
}
