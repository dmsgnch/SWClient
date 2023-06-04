using Assets.Scripts.Managers;
using Assets.Scripts.ViewModels;
using Assets.View.Abstract;
using Components;
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

        [SerializeField] private GameObject ResourcesInfoPanelPrefab;
		[SerializeField] private GameObject SoldiersInfoPanelPrefab;
		[SerializeField] private GameObject ResearchShipInfoPanelPrefab;
		[SerializeField] private GameObject ColonizeShipInfoPanelPrefab;
		

        private HUDViewModel _hudViewModel;

		private void Awake()
		{
            MenuButton.onClick.AddListener(MenuButton_Click);
            AddHoverListeners(ResourcesPanel, OnResourcesPanelHoverEnter, OnResourcesPanelHoverExit);
			AddHoverListeners(SoldiersPanel, OnSoldiersPanelHoverEnter, OnSoldiersPanelHoverExit);
			AddHoverListeners(ResearchShipPanel, OnResearchShipPanelHoverEnter, OnResearchShipPanelHoverExit);
			AddHoverListeners(ColonizeShipPanel, OnColonizeShipPanelHoverEnter, OnColonizeShipPanelHoverExit);
			
		}
		private void MenuButton_Click()
		{
			_hudViewModel.ToMenu();
		}
        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                //TODO: Add confirm window	
                _hudViewModel.ToMenu();
            }
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


		public void UpdateSession()
		{
			_hudViewModel.GetSessionRequestCreate();
		}

		public void UpdateHero()
		{
			_hudViewModel.GetHeroRequestCreate();
		}

		public void UpdateHUDValues()
		{
			_hudViewModel.UpdatePanelTexts(ResourcesPanel, 
				SoldiersPanel, ResearchShipPanel, ColonizeShipPanel, TurnPanel);
		}

		private void OnEnable()
		{
			if (_hudViewModel is null) return;
			UpdateSession();
            //_hudViewModel.SetTurnButtonUnactiveStatus(TurnPanel);
            //_hudViewModel.SetTurnButtonActiveStatus(TurnPanel);
        }
		public void SetTurnPanelTimer(int time) {
			_hudViewModel.SetTurnPanelTimer(TurnPanel, time);
		}


		private void OnDisable()
		{
			//HUD_values.OnValuesChanged -= UpdateTextValues;
		}

		protected override void OnBind(HUDViewModel hudViewModel)
		{
			_hudViewModel = hudViewModel;
            
        }
	}
}
