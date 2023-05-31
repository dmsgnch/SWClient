using Assets.Resourses.MainGame;
using Assets.Scripts.LocalManagers._2_MainGameScripts.RequestsAndResponses.Requests;
using LocalManagers.RegisterLoginRequests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using ViewModels.Abstract;
using Object = UnityEngine.Object;

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


        public void UpdatePanelTexts(GameObject resourcesPanel, GameObject soldiersPanel,
            GameObject researchShipsPanel, GameObject colonizationShipsPanel, GameObject turnPanel) {
            resourcesPanel.GetComponentInChildren<TMP_Text>().text = HUD_values.totalNumResourses.ToString();
            soldiersPanel.GetComponentInChildren<TMP_Text>().text = HUD_values.totalNumSoldiers.ToString();
            researchShipsPanel.GetComponentInChildren<TMP_Text>().text = HUD_values.totalNumResearchShips.ToString();
            colonizationShipsPanel.GetComponentInChildren<TMP_Text>().text = HUD_values.totalNumColonizationShips.ToString();
            TMP_Text[] turnTexts = turnPanel.GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text tmpText in turnTexts)
            {
                switch (tmpText.gameObject.name)
                {
                    case "txt_leftTimeValue":
                        tmpText.text = HUD_values.timeLeft.ToString();
                        break;
                    case "txt_currentTurnValue":
                        tmpText.text = HUD_values.currentTurnHeroName;
                        break;
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

			var createGetHeroRequest = CreateGetHeroRequestObject.AddComponent<GetHeroRequestCreator>();

			createGetHeroRequest.CreateRequest();
		}

		public void CreateResourcePanel(GameObject resourcesInfoPanelPrefab, Transform parent)
		{
            if(_resourcesInfoPanel is null)
            {
                _resourcesInfoPanel = MonoBehaviour.Instantiate(resourcesInfoPanelPrefab, parent);
            }

            TMP_Text[] tmpTextComponents = _resourcesInfoPanel.GetComponentsInChildren<TMP_Text>();

            foreach (TMP_Text tmpText in tmpTextComponents)
            {
                if (tmpText.gameObject.name == "txt_resourcesValue") 
                    tmpText.text = HUD_values.totalNumResourses.ToString();
            }
        }

		public void CreateSoldiersPanel(GameObject soldiersInfoPanelPrefab, Transform parent)
		{
            if(_soldiersInfoPanel is null)
            {
                _soldiersInfoPanel = MonoBehaviour.Instantiate(soldiersInfoPanelPrefab,parent);
            }

            TMP_Text[] tmpTextComponents = _soldiersInfoPanel.GetComponentsInChildren<TMP_Text>();

            foreach (TMP_Text tmpText in tmpTextComponents)
            {
                switch (tmpText.gameObject.name)
                {
                    case "txt_totalSoldiersValue":
                        tmpText.text = HUD_values.totalNumSoldiers.ToString();
                        break;
                    case "txt_usedSoldiersValue":
                        tmpText.text = HUD_values.usedNumSoldiers.ToString();
                        break;
                }
            }
        }

        public void CreateResearchShipPanel(GameObject researchShipInfoPanelPrefab, Transform parent)
        {
            if(_researchShipInfoPanel is null)
            {
                _researchShipInfoPanel = MonoBehaviour.Instantiate(researchShipInfoPanelPrefab,parent);
            }

            TMP_Text[] tmpTextComponents = _researchShipInfoPanel.GetComponentsInChildren<TMP_Text>();

            foreach (TMP_Text tmpText in tmpTextComponents)
            {
              switch (tmpText.gameObject.name) {
                    case "txt_totalShipsValue":
                        tmpText.text = HUD_values.totalNumResearchShips.ToString();
                        break;
                    case "txt_usedShipsValue":
                        tmpText.text = HUD_values.usedNumResearchShips.ToString();
                        break;
                }
            }
        }

        public void CreateColonizeShipPanel(GameObject colonizeShipInfoPanelPrefab, Transform parent)
        {
            if(_colonizeShipInfoPanel is null)
            {
                _colonizeShipInfoPanel = MonoBehaviour.Instantiate(colonizeShipInfoPanelPrefab, parent);
            }

            TMP_Text[] tmpTextComponents = _colonizeShipInfoPanel.GetComponentsInChildren<TMP_Text>();

            foreach (TMP_Text tmpText in tmpTextComponents)
            {
                switch (tmpText.gameObject.name)
                {
                    case "txt_totalShipsValue":
                        tmpText.text = HUD_values.totalNumColonizationShips.ToString();
                        break;
                    case "txt_usedShipsValue":
                        tmpText.text = HUD_values.usedNumColonizationShips.ToString();
                        break;
                }
            }
        }

		public void DeleteResourcePanel()
		{
			Object.Destroy(_resourcesInfoPanel);
            _resourcesInfoPanel= null;
        }

        public void DeleteSoldiersPanel()
        {
            Object.Destroy(_soldiersInfoPanel);
            _soldiersInfoPanel= null;
        }

        public void DeleteResearchShipPanel()
        {
            Object.Destroy(_researchShipInfoPanel);
            _researchShipInfoPanel= null;
        }

        public void DeleteColonizeShipPanel()
        {
            Object.Destroy(_colonizeShipInfoPanel);
            _colonizeShipInfoPanel= null;
        }

       

    }
}
