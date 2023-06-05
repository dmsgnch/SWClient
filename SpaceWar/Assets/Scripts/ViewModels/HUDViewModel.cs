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
                    TurnPanelTimeOut();
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

        private void TurnPanelTimeOut()
        {
            Debug.Log("OUT OF TIME");
        }
		public void ToMenu()
		{
			//Time.timeScale = 0f;
            GameManager.Instance.ChangeState(GameState.MainGameMenu);
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
			button.enabled = false;
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
            button.enabled = true;
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

		public async void SetHeroNewValues(Hero hero, GameObject changesLinePrefab)
		{
			var ChangesLines = new List<string[]>();

			var oldHero = GameManager.Instance.HeroDataStore;
            GameManager.Instance.HeroDataStore.Name = hero.Name;
			var ResDiff = hero.Resourses - oldHero.Resourses;
			if (ResDiff != 0)
			{
                var symbol = ResDiff > 0 ? "+" : "";
                var s = Enumerable.Repeat(string.Empty, 4).ToArray(); s[0] = $"Resources\n({symbol}{ResDiff})";
				ChangesLines.Add(s);
            }
			GameManager.Instance.HeroDataStore.Resourses = hero.Resourses;

            var SoldiersLimDiff = hero.SoldiersLimit - oldHero.SoldiersLimit;
			if (SoldiersLimDiff != 0) {
                var symbol = SoldiersLimDiff > 0 ? "+" : "";
                if (ChangesLines.Count == 0)
				{
                    var s = Enumerable.Repeat(string.Empty, 4).ToArray();
                    s[1] = $"Soldiers limit\n({symbol}{SoldiersLimDiff})";
					ChangesLines.Add(s);
                }
				else {
					ChangesLines[0][1] = $"Soldiers limit\n({symbol}{SoldiersLimDiff})";
                }
			}
            GameManager.Instance.HeroDataStore.SoldiersLimit = hero.SoldiersLimit;

            var AvailableSoldiersDiff = hero.AvailableSoldiers - oldHero.AvailableSoldiers;
            if (AvailableSoldiersDiff != 0)
            {
                var symbol = AvailableSoldiersDiff > 0 ? "+" : "";
				if (ChangesLines.Count == 1 && ChangesLines[0][1] == string.Empty)
				{
					ChangesLines[0][1] = $"Available soldiers\n({symbol}{AvailableSoldiersDiff})";
				}
				else if (ChangesLines.Count == 2)
				{
					ChangesLines[1][1] = $"Available soldiers\n({symbol}{AvailableSoldiersDiff})";
				}
				else {
                    var s = Enumerable.Repeat(string.Empty, 4).ToArray(); s[1] = $"Available soldiers\n({symbol}{AvailableSoldiersDiff})";
                    ChangesLines.Add(s);
                }
            }
            GameManager.Instance.HeroDataStore.AvailableSoldiers = hero.AvailableSoldiers;

            var ResearchShipLimitDiff = hero.ResearchShipLimit - oldHero.ResearchShipLimit;
            if (ResearchShipLimitDiff != 0)
            {
                var symbol = ResearchShipLimitDiff > 0 ? "+" : "";
                if (ChangesLines.Count == 0)
                {
                    var s = Enumerable.Repeat(string.Empty, 4).ToArray(); s[2] = $"Research ship limit\n({symbol}{ResearchShipLimitDiff})";
                    ChangesLines.Add(s);
                }
                else
                {
                    ChangesLines[0][2] = $"Research ship limit\n({symbol}{ResearchShipLimitDiff})";
                }
            }
            GameManager.Instance.HeroDataStore.ResearchShipLimit = hero.ResearchShipLimit;

            var AvailableResearchShipsDiff = hero.AvailableResearchShips - oldHero.AvailableResearchShips;
            if (AvailableResearchShipsDiff != 0)
            {
                var symbol = AvailableResearchShipsDiff > 0 ? "+" : "";
                if (ChangesLines.Count == 1 && ChangesLines[0][2] == string.Empty)
                {					
                    ChangesLines[0][2] = $"Available research ships\n({symbol}{AvailableResearchShipsDiff})";
                }
                else if (ChangesLines.Count == 2)
                {
                    ChangesLines[1][2] = $"Available research ships\n({symbol}{AvailableResearchShipsDiff})";
                }
                else
                {
                    var s = Enumerable.Repeat(string.Empty, 4).ToArray(); s[2] = $"Available research ships\n({symbol}{AvailableResearchShipsDiff})";
                    ChangesLines.Add(s);
                }
            }
            GameManager.Instance.HeroDataStore.AvailableResearchShips = hero.AvailableResearchShips;

            var ColonizationShipLimitDiff = hero.ColonizationShipLimit - oldHero.ColonizationShipLimit;
            if (ColonizationShipLimitDiff != 0)
            {
                var symbol = ColonizationShipLimitDiff > 0 ? "+" : "";
                if (ChangesLines.Count == 0)
                {
                    var s = Enumerable.Repeat(string.Empty, 4).ToArray(); s[3] = $"Colonization ship limit\n({symbol}{ColonizationShipLimitDiff})";
                    ChangesLines.Add(s);
                }
                else
                {
                    ChangesLines[0][3] = $"Colonization ship limit\n({symbol}{ColonizationShipLimitDiff})";
                }
            }
            GameManager.Instance.HeroDataStore.ColonizationShipLimit = hero.ColonizationShipLimit;

            var AvailableColonizationShipsDiff = hero.AvailableColonizationShips - oldHero.AvailableColonizationShips;
            if (AvailableColonizationShipsDiff != 0)
            {
                var symbol = AvailableColonizationShipsDiff > 0 ? "+" : "";
                if (ChangesLines.Count == 1 && ChangesLines[0][3] == string.Empty)
                {
                    ChangesLines[0][3] = $"Available colonization ships\n({symbol}{AvailableColonizationShipsDiff})";
                }
                else if (ChangesLines.Count == 2)
                {
                    ChangesLines[1][3] = $"Available colonization ships\n({symbol}{AvailableColonizationShipsDiff})";
                }
                else
                {
                    var s = Enumerable.Repeat(string.Empty, 4).ToArray();
					s[3] = $"Available colonization ships\n({symbol}{AvailableColonizationShipsDiff})";
                    ChangesLines.Add(s);
                }
            }
            GameManager.Instance.HeroDataStore.AvailableColonizationShips = hero.AvailableColonizationShips;

			if (ChangesLines.Count != 0) await ShowChangesLines(ChangesLines, changesLinePrefab);
		}

        private async Task ShowChangesLines(List<string[]> lines, GameObject changesLinePrefab)
        {
            List<GameObject> GOes = new List<GameObject>();

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                var lineGO = GameObject.Instantiate(changesLinePrefab, GameObject.Find("cnvs_HUD").transform);
                lineGO.name += "_" + i.ToString();
                GOes.Add(lineGO);
                lineGO.transform.position += Vector3.down * i * 45;
				var texts = lineGO.transform.GetComponentsInChildren<TMP_Text>();
                for (int j = 0; j < line.Length; j++)
                {
					texts[j].text = line[j];
                    if (line[j].Contains('-'))
                        texts[j].color = UnityEngine.Color.red;
                    else
                        texts[j].color = UnityEngine.Color.green;
                }
            }

            await Task.Delay(3000);

            foreach (var GO in GOes)
            {
                GameObject.Destroy(GO);
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
