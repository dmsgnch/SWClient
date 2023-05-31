using Assets.Scripts.LocalManagers._2_MainGameScripts.RequestsAndResponses.Requests;
using LocalManagers.RegisterLoginRequests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public void UpdateStatusBar(ref GameObject statusBar) {
		
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
		}

		public void CreateSoldiersPanel(GameObject soldiersInfoPanelPrefab, Transform parent)
		{
            if(_soldiersInfoPanel is null)
            {
                _soldiersInfoPanel = MonoBehaviour.Instantiate(soldiersInfoPanelPrefab,parent);
            }
        }

        public void CreateResearchShipPanel(GameObject researchShipInfoPanelPrefab, Transform parent)
        {
            if(_researchShipInfoPanel is null)
            {
                _researchShipInfoPanel = MonoBehaviour.Instantiate(researchShipInfoPanelPrefab,parent);
            }
        }

        public void CreateColonizeShipPanel(GameObject colonizeShipInfoPanelPrefab, Transform parent)
        {
            if(_colonizeShipInfoPanel is null)
            {
                _colonizeShipInfoPanel = MonoBehaviour.Instantiate(colonizeShipInfoPanelPrefab, parent);
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
