using Assets.Scripts.Managers;
using Assets.Scripts.ViewModels;
using Assets.View.Abstract;
using Components;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
	public class HUDView : AbstractScreen<HUDViewModel>
	{
		[SerializeField] private GameObject TurnPanel;
		[SerializeField] private GameObject ResourcesPanel;
		[SerializeField] private GameObject SoldiersPanel;
		[SerializeField] private GameObject ResearchShipPanel;
		[SerializeField] private GameObject ColonizeShipPanel;
		[SerializeField] private Button MenuButton;
		[SerializeField] private Button NextTurnButton;
        [SerializeField] private GameObject playerList;

        [SerializeField] private GameObject playerName_Prefab;
        [SerializeField] private GameObject ResourcesInfoPanelPrefab;
		[SerializeField] private GameObject SoldiersInfoPanelPrefab;
		[SerializeField] private GameObject ResearchShipInfoPanelPrefab;
		[SerializeField] private GameObject ColonizeShipInfoPanelPrefab;
        [SerializeField] private GameObject ChangeMessagePrefab;

		private bool firstValuesSet = true;

        private HUDViewModel _hudViewModel;

		private void Awake()
		{
			MenuButton.onClick.AddListener(OnMenuButtonClick);
			NextTurnButton.onClick.AddListener(OnNextTurnButtonClick);
			AddHoverListeners(ResourcesPanel, OnResourcesPanelHoverEnter, OnResourcesPanelHoverExit);
			AddHoverListeners(SoldiersPanel, OnSoldiersPanelHoverEnter, OnSoldiersPanelHoverExit);
			AddHoverListeners(ResearchShipPanel, OnResearchShipPanelHoverEnter, OnResearchShipPanelHoverExit);
			AddHoverListeners(ColonizeShipPanel, OnColonizeShipPanelHoverEnter, OnColonizeShipPanelHoverExit);
		}

		private void OnMenuButtonClick()
		{
			PlayButtonClickSound();

			_hudViewModel.ToMenu();
		}

		private void OnNextTurnButtonClick()
		{
			PlayButtonClickSound();

			_hudViewModel.SendNextTurnRequest();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))			
				_hudViewModel.ToMenu();
			
			_hudViewModel.ReduceTurnPanelTime(TurnPanel, Time.deltaTime);
		}

		private void AddHoverListeners(GameObject panel, UnityAction<PointerEventData> onEnter, UnityAction<PointerEventData> onExit)
		{
			EventTrigger trigger = panel.GetComponent<EventTrigger>();
			if (trigger == null)
			{
				trigger = panel.AddComponent<EventTrigger>();
			}

			EventTrigger.Entry enterEntry = new EventTrigger.Entry();
			enterEntry.eventID = EventTriggerType.PointerEnter;
			enterEntry.callback.AddListener((eventData) => { onEnter((PointerEventData)eventData); });
			trigger.triggers.Add(enterEntry);

			EventTrigger.Entry exitEntry = new EventTrigger.Entry();
			exitEntry.eventID = EventTriggerType.PointerExit;
			exitEntry.callback.AddListener((eventData) => { onExit((PointerEventData)eventData); });
			trigger.triggers.Add(exitEntry);
		}

		#region Event handlers

		public void OnResourcesPanelHoverEnter(PointerEventData eventData)
		{
			_hudViewModel.CreateResourcePanel(ResourcesInfoPanelPrefab, gameObject.transform);
		}

		public void OnResourcesPanelHoverExit(PointerEventData eventData)
		{
			_hudViewModel.DeleteResourcePanel();
		}

		public void OnSoldiersPanelHoverEnter(PointerEventData eventData)
		{
			_hudViewModel.CreateSoldiersPanel(SoldiersInfoPanelPrefab, gameObject.transform);
		}

		public void OnSoldiersPanelHoverExit(PointerEventData eventData)
		{
			_hudViewModel.DeleteSoldiersPanel();
		}

		public void OnResearchShipPanelHoverEnter(PointerEventData eventData)
		{
			_hudViewModel.CreateResearchShipPanel(ResearchShipInfoPanelPrefab, gameObject.transform);
		}

		public void OnResearchShipPanelHoverExit(PointerEventData eventData)
		{
			_hudViewModel.DeleteResearchShipPanel();
		}

		public void OnColonizeShipPanelHoverEnter(PointerEventData eventData)
		{
			_hudViewModel.CreateColonizeShipPanel(ColonizeShipInfoPanelPrefab, gameObject.transform);
		}

		public void OnColonizeShipPanelHoverExit(PointerEventData eventData)
		{
			_hudViewModel.DeleteColonizeShipPanel();
		}

		#endregion
		
		public void callResourcesChangedPanel(string value) {
            if (firstValuesSet) return;
            _hudViewModel.ShowChangePanel(value, ChangeMessagePrefab, ResourcesPanel);
            UpdateHeroHudValues();
        }
        public void callSoldiersChangedPanel(string value)
        {
            if (firstValuesSet) return;
            _hudViewModel.ShowChangePanel(value, ChangeMessagePrefab, SoldiersPanel);
            UpdateHeroHudValues();
        }
        public void callResearchShipsChangedPanel(string value)
        {
			if (firstValuesSet) return;
            _hudViewModel.ShowChangePanel(value, ChangeMessagePrefab, ResearchShipPanel);
            UpdateHeroHudValues();
        }
        public void callColonizeShipsChangedPanel(string value)
        {
            if (firstValuesSet) return;
            _hudViewModel.ShowChangePanel(value, ChangeMessagePrefab, ColonizeShipPanel);
            UpdateHeroHudValues();
        }
        
        public void UpdatePlayerList()
        {
			while(playerList.transform.childCount > 0)
			{
				DestroyImmediate(playerList.transform.GetChild(0).gameObject);
			}
            var sessionDataStore = GameManager.Instance.SessionDataStore;
            foreach (var player in sessionDataStore.PanelHeroForms)
            {
                GameObject playerPanel = Instantiate(playerName_Prefab, playerList.transform);
                playerPanel.GetComponent<TMP_Text>().text = player.HeroName;
            }
        }
        #region Requests senders

        public void UpdateSessionRequest()
		{
			_hudViewModel.GetSessionRequestCreate();

			//In response execute UpdateHeroRequest
		}

		public void UpdateHeroRequest()
		{
			_hudViewModel.GetHeroRequestCreate();
		}

		#endregion

		#region Update HUD values

		public void UpdateHUDValues()
		{
			UpdateHeroHudValues();
			UpdateSessionHudValues();
			firstValuesSet = false;
		}

		public void UpdateHeroHudValues()
		{
			_hudViewModel.UpdateHeroDataPanelTexts(ResourcesPanel, SoldiersPanel, ResearchShipPanel, ColonizeShipPanel);
		}

		public void UpdateSessionHudValues()
		{
			_hudViewModel.UpdateSessionDataPanelTexts(TurnPanel);
		}

		#endregion

		#region Setters

		public void SetHeroNewValues(Hero hero)
		{
			_hudViewModel.SetHeroNewValues(hero);
		}

		public void SetTurnPanelTimer(int time)
		{
			_hudViewModel.SetTurnPanelTimer(TurnPanel, time);
		}

		#endregion

		private void OnEnable()
		{
			if (_hudViewModel is null || transform.childCount.Equals(0)) return;

			UpdateSessionRequest();
		}

		protected override void OnBind(HUDViewModel hudViewModel)
		{
			_hudViewModel = hudViewModel;
		}
	}
}
