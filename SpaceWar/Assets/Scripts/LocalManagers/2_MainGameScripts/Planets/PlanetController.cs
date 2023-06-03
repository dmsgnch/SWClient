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

public class PlanetController : MonoBehaviour
{
	public float rotationSpeed = 10f;
	public Planet planet;
	public GameObject planetPrefab;
	public GameObject ButtonPrefab;
	public GameObject InfoPanelPrefab;
	public float timeThreshold = 1f;

	private GameObject actMenu;
	private MonoBehaviour[] cameraScripts;
	private bool actMenuEnabled = false;
	private List<string> actionNames;
	private float hoverTime;
	private bool createNewObject;
	private GameObject InfoPanel = null;
	private byte CountOfButtons { get; set; } = 0;

	private void Start()
	{
		cameraScripts = GameObject.Find("Look_Camera").GetComponents<MonoBehaviour>();
		actMenu = GameObject.Find("ActionsMenu");
	}
	private void Update()
	{
		if (createNewObject && InfoPanel is null)
		{
			CreatePlanetInfoPanel();
			createNewObject = false;
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
					OpearationChoosing();
				}
			}
			else
			{
				DestroyActMenu();
			}
		}
	}

	void CreatePlanetInfoPanel()
	{
		InfoPanel = Instantiate(InfoPanelPrefab, GameObject.Find("cnvs_HUD").transform);
		InfoPanel.transform.position = Input.mousePosition;

		for (int i = 0; i < InfoPanel.transform.childCount; i++)
		{
			var child = InfoPanel.transform.GetChild(i);
			switch (child.name)
			{
				case "Name":
					child.transform.GetComponentsInChildren<TMP_Text>()
						.FirstOrDefault(t => t.name == "Value").text = planet.PlanetName;
					break;
				case "Fortification":
					child.transform.GetComponentsInChildren<TMP_Text>()
						.FirstOrDefault(t => t.name == "Value").text = planet.FortificationLevel.ToString();
					break;
				case "Size":
					child.transform.GetComponentsInChildren<TMP_Text>()
						.FirstOrDefault(t => t.name == "Value").text = planet.Size.ToString();
					break;
				case "Status":
					child.transform.GetComponentsInChildren<TMP_Text>()
						.FirstOrDefault(t => t.name == "Value").text = planet.Status.ToString();
					break;
				case "Resources":
					child.transform.GetComponentsInChildren<TMP_Text>()
						.FirstOrDefault(t => t.name == "Value").text = "res-type";
					break;
				default:
					throw new DataException();
			}
		}
	}

	/// <summary>
	/// Create buttons one uder the other
	/// </summary>
	/// <param name="buttonName"></param>
	/// <param name="onClick"></param>
	private void CreateActMenu(string buttonName, out Button onClick)
	{
		if (CountOfButtons.Equals(0))
			actMenu.transform.position = Input.mousePosition;

		GameObject buttonObject = Instantiate(ButtonPrefab, actMenu.transform);
		Button button = buttonObject.GetComponent<Button>();

		TMP_Text buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
		buttonText.text = actionNames[CountOfButtons];

		buttonObject.transform.localPosition = new Vector3(0f, -CountOfButtons * 30f, 0f);

		onClick = button;

		CountOfButtons++;

		if (CountOfButtons.Equals(0))
		{
			actMenuEnabled = true;
			DisableScripts();
		}
	}

	/// <summary>
	/// Perform sugery and binding according to planet status
	/// </summary>
	/// <exception cref="DataException"></exception>
	private void OpearationChoosing()
	{
		var planetsView = GetPlanetsView();

		switch (planet.Status)
		{
			case PlanetStatus.Known:
				CreateActMenu("Research", out Button researchButton);
				researchButton.onClick.AddListener(() => planetsView.Research(planet));
				researchButton.onClick.AddListener(DestroyActMenu);
				break;

			case PlanetStatus.Researched:
				CreateActMenu("Colonize", out Button colonizeButton);
				colonizeButton.onClick.AddListener(() => planetsView.Colonize(planet));
				colonizeButton.onClick.AddListener(DestroyActMenu);
				break;

			case PlanetStatus.Enemy:
				if (IsAvaibleToAtack())
				{
					CreateActMenu("Attack", out Button attackButton);
					attackButton.onClick.AddListener(() => planetsView.Attack(planet));
					attackButton.onClick.AddListener(DestroyActMenu);
				}
				break;

			case PlanetStatus.Colonized:
				OpearationDefenceChoosing(planetsView);
				break;
			default:
				throw new DataException();
		}

		if(IsDefencing(planetsView))
		{
			CreateActMenu("Send soldiers to defend", out Button defenceButton);
			defenceButton.onClick.AddListener(() => planetsView.Defence(planet));
			defenceButton.onClick.AddListener(DestroyActMenu);
		}
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
				CreateActMenu("Build light defence (cost)", out Button liteDefButton);
				liteDefButton.onClick.AddListener(() => planetsView.BuiltLightDefence(planet));
				liteDefButton.onClick.AddListener(DestroyActMenu);
				break;

			case Fortification.Weak:
				CreateActMenu("Build medium defence (cost)", out Button mediumDefButton);
				mediumDefButton.onClick.AddListener(() => planetsView.BuiltMidleDefence(planet));
				mediumDefButton.onClick.AddListener(DestroyActMenu);
				break;

			case Fortification.Reliable:
				CreateActMenu("Build strong defence (cost)", out Button strongDefButton);
				strongDefButton.onClick.AddListener(() => planetsView.BuiltStrongDefence(planet));
				strongDefButton.onClick.AddListener(DestroyActMenu);
				break;

			default:
				break;
		}
	}

	/// <summary>
	/// Check that any connection has a battle where the currect planet is being defended
	/// </summary>
	/// <param name="planetsView"></param>
	/// <returns>True if the specified connection is found</returns>
	private bool IsDefencing(PlanetsView planetsView)
	{
		//TODO: Add checking that connection has battle where "planet" is defencing
		return GameManager.Instance.HeroDataStore.HeroMapView.Connections.Any(c => 
		c.FromPlanetId.Equals(planet.Id) || c.ToPlanetId.Equals(planet.Id));
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

	private bool IsAvaibleToAtack()
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
		CountOfButtons = 0;
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
		createNewObject = false;
	}

	private void OnMouseExit()
	{
		Destroy(InfoPanel);
		InfoPanel = null;
		Debug.Log($"info panel destroyed");
		hoverTime = 0f;
		createNewObject = false;
	}

	private void OnMouseOver()
	{
		if (Time.time - hoverTime >= timeThreshold)
		{
			createNewObject = true;
		}
	}

}
