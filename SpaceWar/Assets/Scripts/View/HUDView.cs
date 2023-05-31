using Assets.Resourses.MainGame;
using Assets.Scripts.ViewModels;
using Assets.View.Abstract;
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
		[SerializeField] private GameObject pnl_Turn;

		[SerializeField] private GameObject ResourcesPanel;
		[SerializeField] private GameObject SoldiersPanel;
		[SerializeField] private GameObject ResearchShipPanel;
		[SerializeField] private GameObject ColonizeShipPanel;

		[SerializeField] private GameObject ResourcesInfoPanelPrefab;
		[SerializeField] private GameObject SoldiersInfoPanelPrefab;
		[SerializeField] private GameObject ResearchShipInfoPanelPrefab;
		[SerializeField] private GameObject ColonizeShipInfoPanelPrefab;

		private HUDViewModel _hudViewModel;

        private void Awake()
		{
            AddHoverListeners(ResourcesPanel, OnResourcesPanelHoverEnter, OnResourcesPanelHoverExit);
			//AddHoverListeners(SoldiersPanel, OnSoldiersPanelHoverEnter, OnSoldiersPanelHoverExit);
			AddHoverListeners(ResearchShipPanel, OnResearchShipPanelHoverEnter, OnResearchShipPanelHoverExit);
			AddHoverListeners(ColonizeShipPanel, OnColonizeShipPanelHoverEnter, OnColonizeShipPanelHoverExit);
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
			_hudViewModel.CreateResourcePanel(ResourcesInfoPanelPrefab,gameObject.transform);
		}

        public void OnResourcesPanelHoverExit(PointerEventData eventData)
		{
			_hudViewModel.DeleteResourcePanel();
		}

        public void OnSoldiersPanelHoverEnter()
        {
            _hudViewModel.CreateSoldiersPanel(SoldiersInfoPanelPrefab, gameObject.transform);
        }

        public void OnSoldiersPanelHoverExit()
		{
			_hudViewModel.DeleteSoldiersPanel();
		}

        public void OnResearchShipPanelHoverEnter(PointerEventData eventData)
		{
			_hudViewModel.CreateResearchShipPanel(ResearchShipInfoPanelPrefab,gameObject.transform);
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

		private void UpdateTextValues() {
			_hudViewModel.UpdatePanelTexts(ResourcesPanel, SoldiersPanel, ResearchShipPanel, ColonizeShipPanel, pnl_Turn);
        }

		private void OnEnable()
		{
			if (_hudViewModel is null) return;
            HUD_values.OnValuesChanged += UpdateTextValues;
            UpdateSession();
		}

        private void OnDisable()
        {
            HUD_values.OnValuesChanged -= UpdateTextValues;
        }

        private void Start()
        {
            _hudViewModel.UpdatePanelTexts(ResourcesPanel, SoldiersPanel, ResearchShipPanel, ColonizeShipPanel, pnl_Turn);
        }

        protected override void OnBind(HUDViewModel hudViewModel)
		{
			_hudViewModel = hudViewModel;
		}
	}
}
