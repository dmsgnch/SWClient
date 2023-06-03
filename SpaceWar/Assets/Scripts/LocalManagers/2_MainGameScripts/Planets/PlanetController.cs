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
using UnityEngine.UIElements;

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
    private MonoBehaviour[] scripts;
    private bool actMenuEnabled = false;
    private List<string> actionNames;
    private float hoverTime;
    private bool createNewPlanetInfoPanel;
    private GameObject InfoPanel = null;
    private UnityEngine.UI.Slider slider;
    private Camera mainCamera;


    private void Start()
    {
        scripts = GameObject.Find("Look_Camera").GetComponents<MonoBehaviour>();
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

        if (createNewPlanetInfoPanel && InfoPanel is null)
        {
            CreateInfoPanel();
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
                    CreateActMenu();
                }
            }
            else
            {
                DestroyActMenu();
            }
        }
    }
    private void UpdateHealthBar() {
        Vector3 planetPosition = transform.position;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(planetPosition);
        Vector3 sliderPosition = screenPosition;

        sliderPosition.y += slider.GetComponent<RectTransform>().rect.height;
        slider.transform.position = sliderPosition + Vector3.up * planet.Size;
    }
    private void CreateHealthBar()
    {
        var sliderGO = Instantiate(HealthBarPrefab, transform.parent);
        slider = sliderGO.GetComponent<UnityEngine.UI.Slider>();
    }

    void CreateInfoPanel() {
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

    void CreateActMenu()
    {
        actionNames = GetButtonsListByPlanetStatus();
        if (actionNames.Count <= 0) return;
        actMenu.transform.position = Input.mousePosition;

		for (int i = 0; i < actionNames.Count; i++)
		{
			GameObject buttonObject = Instantiate(ButtonPrefab, actMenu.transform);
			UnityEngine.UI.Button button = buttonObject.GetComponent<UnityEngine.UI.Button>();

			TMP_Text buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
			buttonText.text = actionNames[i];

			buttonObject.transform.localPosition = new Vector3(0f, -i * 30f, 0f);

			int index = i;
			button.onClick.AddListener(() => OnActionButtonClick(index));
		}
		actMenuEnabled = true;
        DisableScripts();
    }

    private List<string> GetButtonsListByPlanetStatus()
    {
		switch (planet.Status)
		{
			case PlanetStatus.Known:
				return new List<string> { "Research" };
			case PlanetStatus.Researching:
				return new List<string> { };
			case PlanetStatus.Researched:
				return new List<string> { "Colonize" };
			case PlanetStatus.HasStation:
				return new List<string> { };
			case PlanetStatus.Colonizing:
				return new List<string> { };
			case PlanetStatus.Colonized:
                return GetButtonsListByPlanetFortification();
			case PlanetStatus.Enemy:
                if (isAvaibleToAtack()) {
                    return new List<string> { "Attack" };
                }
				return new List<string> { };
			default:
				throw new DataException();
		}
	}

    private bool isAvaibleToAtack() {

        var connections = GameManager.Instance.HeroDataStore.HeroMapView.Connections.Where(c => c.FromPlanetId.Equals(planet.Id) || c.ToPlanetId.Equals(planet.Id));
        var result = connections.Where(c => c.From.OwnerId.Value.Equals(GameManager.Instance.HeroDataStore.HeroId) ||
        c.To.OwnerId.Value.Equals(GameManager.Instance.HeroDataStore.HeroId)).Any();
        return result;
    }

	private List<string> GetButtonsListByPlanetFortification() 
    {
		switch (planet.FortificationLevel)
		{
			case Fortification.None:
				return new List<string> { "Build light defence (cost)" };
			case Fortification.Weak:
				return new List<string> { "Build medium defence (cost)" };
			case Fortification.Reliable:
				return new List<string> { "Build strong defence (cost)" };
			case Fortification.Strong:
				return new List<string> { };
			default:
				throw new DataException();
		}
	}

	private void OnActionButtonClick(int index)
    {
        Debug.Log($"Action button \"{actionNames[index]}\" clicked on planet \"{gameObject.name}\"");
        DestroyActMenu();
    }

    void DestroyActMenu()
    {
        actMenu.transform.DestroyChildren();
        EnableScripts();
        actMenuEnabled = false;
    }

    void DisableScripts()
    {
        foreach (var script in scripts)
        {
            script.enabled = false;
        }
    }

    void EnableScripts()
    {
        foreach (var script in scripts)
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
        Destroy(InfoPanel);
        InfoPanel = null;
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
