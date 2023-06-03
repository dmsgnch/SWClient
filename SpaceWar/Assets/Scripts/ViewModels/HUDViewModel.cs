using Assets.Scripts.LocalManagers._2_MainGameScripts.RequestsAndResponses.Requests;
using Assets.Scripts.Managers;
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
			GameObject researchShipsPanel, GameObject colonizationShipsPanel, GameObject turnPanel)
		{
			resourcesPanel.GetComponentInChildren<TMP_Text>().text = 
				GameManager.Instance.HeroDataStore.Resourses.ToString();
			soldiersPanel.GetComponentInChildren<TMP_Text>().text =
				GameManager.Instance.HeroDataStore.AvailableSoldiers.ToString();
			researchShipsPanel.GetComponentInChildren<TMP_Text>().text = 
				GameManager.Instance.HeroDataStore.AvailableResearchShips.ToString();
			colonizationShipsPanel.GetComponentInChildren<TMP_Text>().text = 
				GameManager.Instance.HeroDataStore.AvailableColonizationShips.ToString();
			TMP_Text[] turnTexts = turnPanel.GetComponentsInChildren<TMP_Text>();
			foreach (TMP_Text tmpText in turnTexts)
			{
				switch (tmpText.gameObject.name)
				{
					case "txt_leftTimeValue":
						int timeLimitView = GameManager.Instance.SessionDataStore.TurnTimeLimit;
                        tmpText.text = timeLimitView.ToString();
						break;
					case "txt_currentTurnValue":
                        int turnNumberView = GameManager.Instance.SessionDataStore.TurnNumber + 1;
                        tmpText.text = turnNumberView.ToString();
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

			var createGetHeroRequest = CreateGetHeroRequestObject.AddComponent<GetHeroRequest>();

			createGetHeroRequest.CreateRequest();
		}

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
	}
}
