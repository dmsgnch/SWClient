using Assets.Scripts.Managers;
using SharedLibrary.Models;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Assets.Scripts.View;
using Components;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using SharedLibrary.Models.Enums;

public class PlanetController : MonoBehaviour
{
	public float rotationSpeed = 10f;
	public Planet planet;
	public GameObject planetPrefab;
	public GameObject ButtonPrefab;
	public GameObject InfoPanelPrefab;
	public float timeThreshold = 1f;
	public GameObject HealthBarPrefab;

	private GameObject actMenu;
	private MonoBehaviour[] cameraScripts;
	private bool actMenuEnabled = false;
	//private List<string> actionNames;
	private float hoverTime;
	private bool createNewPlanetInfoPanel;
	private GameObject PlanetInfoPanel = null;
	private UnityEngine.UI.Slider slider;
	private Camera mainCamera;

	private List<Button> ActionsButtons = new List<Button>();

	private void Start()
	{
		cameraScripts = GameObject.Find("Look_Camera").GetComponents<MonoBehaviour>();
		actMenu = GameObject.Find("ActionsMenu");
		mainCamera = Camera.main;
		CreateHealthBar();
		slider.transform.localScale = Vector3.one * 0.7f;
	}
	private void Update()
	{
		if (slider != null && slider.gameObject.activeSelf)
		{
			UpdateHealthBar();
		}

		if (createNewPlanetInfoPanel && PlanetInfoPanel is null && !actMenuEnabled)
		{
			CreatePlanetInfoPanel();
			createNewPlanetInfoPanel = false;
		}

		transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

		if (Input.GetMouseButtonDown(1))
		{
			if (!actMenuEnabled)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
				{
					GenerateActMenus();
				}
			}
			else
			{
				DestroyActMenu();
			}
		}
	}

	private void UpdateHealthBar()
	{
		Vector3 planetPosition = transform.position;
		Vector3 screenPosition = mainCamera.WorldToScreenPoint(planetPosition);
		Vector3 sliderPosition = screenPosition;

		sliderPosition.y += slider.GetComponent<RectTransform>().rect.height;
		slider.transform.position = sliderPosition + Vector3.up * planet.Size;
	}
	private void CreateHealthBar()
	{
		var sliderGO = Instantiate(HealthBarPrefab, transform.parent);
		if (planet.Health >= planet.HealthLimit) sliderGO.SetActive(false);
		slider = sliderGO.GetComponent<UnityEngine.UI.Slider>();
	}

	void CreatePlanetInfoPanel()
	{
		PlanetInfoPanel = Instantiate(InfoPanelPrefab, GameObject.Find("cnvs_HUD").transform);
		PlanetInfoPanel.transform.position = Input.mousePosition;
		PlanetInfoPanel.AddComponent<HUDCursorFollower>();

		for (int i = 0; i < PlanetInfoPanel.transform.childCount; i++)
		{
			var child = PlanetInfoPanel.transform.GetChild(i);
			switch (child.name)
			{
				case "Name":
					child.transform.GetComponentsInChildren<TMP_Text>()
						.FirstOrDefault(t => t.name == "Value").text = planet.PlanetName;
					break;
				case "Fortification":
					if (planet.Status < PlanetStatus.Researched) break;

					child.transform.GetComponentsInChildren<TMP_Text>()
						.FirstOrDefault(t => t.name == "Value").text = planet.FortificationLevel.ToString();
					break;
				case "Size":
					if (planet.Status < PlanetStatus.Researched) break;

					child.transform.GetComponentsInChildren<TMP_Text>()
						.FirstOrDefault(t => t.name == "Value").text = planet.Size.ToString();
					break;
				case "Status":
					if (planet.Status < PlanetStatus.Researched) break;

					child.transform.GetComponentsInChildren<TMP_Text>()
						.FirstOrDefault(t => t.name == "Value").text = planet.Status.ToString();
					break;
				case "Resources":
					if (planet.Status < PlanetStatus.Researched) break;

					var resourceType = planet.ResourceType is ResourceType.OnlyResources ? "Resources" :
						(planet.ResourceType is ResourceType.ColonizationShip ? "Colonization ships" :
						"Research ships");

					child.transform.GetComponentsInChildren<TMP_Text>()
						.FirstOrDefault(t => t.name == "Value").text = $"{planet.ResourceCount} " +
						$"{resourceType}";
					break;
				default:
					throw new DataException();
			}
		}
	}

	/// <summary>
	/// Create ActionsButtons one uder the other
	/// </summary>
	/// <param name="buttonName"></param>
	/// <param name="onClick"></param>
	private void CreateActMenu(string buttonName, out Button onClick)
	{
		if (ActionsButtons.Count.Equals(0))
			actMenu.transform.position = Input.mousePosition;


		GameObject buttonObject = Instantiate(ButtonPrefab, actMenu.transform);
		Button button = buttonObject.GetComponent<Button>();

		TMP_Text buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
		buttonText.text = buttonName;

		buttonObject.transform.localPosition = new Vector3(0f, (-ActionsButtons.Count * 33f) - 30f, 0f);

		onClick = button;

		ActionsButtons.Add(button);
		if (ActionsButtons.Count.Equals(1))
		{
			actMenuEnabled = true;
			DisableScripts();
		}
	}

	/// <summary>
	/// Perform sugery and binding according to planet status
	/// </summary>
	/// <exception cref="DataException"></exception>
	private void GenerateActMenus()
	{
		var planetsView = GetPlanetsView();

		switch (planet.Status)
		{
			case PlanetStatus.Known:
				CreateActMenu("Research (R: 10)", out Button researchButton);
				researchButton.onClick.AddListener(DestroyActMenu);
				researchButton.onClick.AddListener(() => planetsView.Research(planet));
				break;

			case PlanetStatus.Researched:
				CreateActMenu("Colonize (R: 30)", out Button colonizeButton);
				colonizeButton.onClick.AddListener(DestroyActMenu);
				colonizeButton.onClick.AddListener(() => planetsView.Colonize(planet));
				break;

			case PlanetStatus.Enemy:
				if (IsAvaibleToAttack())
				{
					CreateActMenu("Attack", out Button attackButton);
					attackButton.onClick.AddListener(DestroyActMenu);
					attackButton.onClick.AddListener(() => planetsView.Attack(planet));
				}
				break;

			case PlanetStatus.Colonized:
				OpearationDefenceChoosing(planetsView);
				break;

			default:
				throw new DataException();
		}

		if (IsDefencing(planetsView))
		{
			CreateActMenu("Send soldiers to defend", out Button defenceButton);
			defenceButton.onClick.AddListener(DestroyActMenu);
			defenceButton.onClick.AddListener(() => planetsView.Defence(planet));
		}
		ResizeButtons();
	}

	/// <summary>
	/// Perform sugery and binding according to planet defence status
	/// </summary>
	/// <param name="planetsView"></param>
	private void OpearationDefenceChoosing(PlanetsView planetsView)
	{
		switch (planet.FortificationLevel)
		{
			case Fortification.None:
				CreateActMenu("Build light defence (R: later)", out Button liteDefButton);
				AddListeners(liteDefButton);
				break;

			case Fortification.Weak:
				CreateActMenu("Build medium defence (R: later)", out Button mediumDefButton);
				AddListeners(mediumDefButton);
				break;

			case Fortification.Reliable:
				CreateActMenu("Build strong defence (R: later)", out Button strongDefButton);
				AddListeners(strongDefButton);
				break;

			default:
				break;
		}

		void AddListeners(Button button)
		{
			button.onClick.AddListener(DestroyActMenu);
			button.onClick.AddListener(() => planetsView.BuildDefence(planet));
		}
	}

	private void ResizeButtons()
	{
		float maxWidth = 0f;

		// Ќайти максимальную ширину текста среди всех кнопок
		maxWidth = ActionsButtons.Max(b => b.GetComponentInChildren<TMP_Text>().preferredWidth);

		// ”становить одинаковую ширину дл€ всех кнопок
		foreach (Button button in ActionsButtons)
		{
			RectTransform buttonRect = button.GetComponent<RectTransform>();
			buttonRect.sizeDelta = new Vector2(maxWidth, buttonRect.sizeDelta.y);
			RectTransform textRect = button.GetComponentInChildren<TMP_Text>().GetComponent<RectTransform>();
			textRect.sizeDelta = new Vector2(maxWidth, textRect.sizeDelta.y);
		}
	}

	/// <summary>
	/// Check that any connection has a battle where the currect planet is being defended
	/// </summary>
	/// <param name="planetsView"></param>
	/// <returns>True if the specified connection is found</returns>
	private bool IsDefencing(PlanetsView planetsView)
	{
		return GameManager.Instance.BattleDataStore.Battles?.Any(b =>
		b.AttackedPlanetId.Equals(planet.Id)) ?? false;
	}

	/// <summary>
	/// Find PlanetsView component in hierarchy
	/// </summary>
	/// <returns>Finded view model</returns>
	/// <exception cref="Exception"></exception>
	private PlanetsView GetPlanetsView()
	{
		return FindFirstObjectByType<PlanetsView>() ??
			throw new Exception("PlanetsView was not found");
	}

	private bool IsAvaibleToAttack()
	{
		var connections =
			GameManager.Instance.HeroDataStore.HeroMapView.Connections.Where(c =>
			c.FromPlanetId.Equals(planet.Id) || c.ToPlanetId.Equals(planet.Id));

		var result = connections.Where(c => c.From.OwnerId.Value.Equals(GameManager.Instance.HeroDataStore.HeroId) ||
		c.To.OwnerId.Value.Equals(GameManager.Instance.HeroDataStore.HeroId)).Any();
		return result;
	}

	void DestroyActMenu()
	{
		actMenu.transform.DestroyChildren();
		EnableScripts();
		actMenuEnabled = false;
		ActionsButtons.Clear();
	}

	void DisableScripts()
	{
		foreach (var script in cameraScripts)
		{
			script.enabled = false;
		}
	}

	void EnableScripts()
	{
		foreach (var script in cameraScripts)
		{
			script.enabled = true;
		}
	}

	private void OnMouseEnter()
	{
		hoverTime = Time.time;
		createNewPlanetInfoPanel = false;
	}

	private void OnMouseExit()
	{
		Destroy(PlanetInfoPanel);
		PlanetInfoPanel = null;
		hoverTime = 0f;
		createNewPlanetInfoPanel = false;
	}

	private void OnMouseOver()
	{
		if (Time.time - hoverTime >= timeThreshold)
		{
			createNewPlanetInfoPanel = true;
		}
	}

}
