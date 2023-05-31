using Assets.Scripts.ViewModels;
using Assets.View.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			AddHoverListeners(SoldiersPanel, OnSoldiersPanelHoverEnter, OnSoldiersPanelHoverExit);
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

		private void OnResourcesPanelHoverEnter(PointerEventData eventData)
		{
			_hudViewModel.CreateResourcePanel(ResourcesInfoPanelPrefab);
		}

		private void OnResourcesPanelHoverExit(PointerEventData eventData)
		{
			_hudViewModel.DeleteResourcePanel();
		}

		private void OnSoldiersPanelHoverEnter(PointerEventData eventData)
		{
			_hudViewModel.CreateSoldiersPanel(SoldiersInfoPanelPrefab);
		}

		private void OnSoldiersPanelHoverExit(PointerEventData eventData)
		{
			_hudViewModel.DeleteSoldiersPanel();
		}

		private void OnResearchShipPanelHoverEnter(PointerEventData eventData)
		{
			_hudViewModel.CreateResearchShipPanel(ResearchShipInfoPanelPrefab);
		}

		private void OnResearchShipPanelHoverExit(PointerEventData eventData)
		{
			_hudViewModel.DeleteResearchShipPanel();
		}

		private void OnColonizeShipPanelHoverEnter(PointerEventData eventData)
		{
			_hudViewModel.CreateColonizeShipPanel(ColonizeShipInfoPanelPrefab);
		}

		private void OnColonizeShipPanelHoverExit(PointerEventData eventData)
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

		private void OnEnable()
		{
			if (_hudViewModel is null) return;

			UpdateSession();
		}

		protected override void OnBind(HUDViewModel hudViewModel)
		{
			_hudViewModel = hudViewModel;
		}
	}
}
