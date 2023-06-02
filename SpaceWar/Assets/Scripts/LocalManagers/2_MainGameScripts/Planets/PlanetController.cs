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

public class PlanetController : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public Planet planet;
    public GameObject planetPrefab;
    public GameObject ButtonPrefab;
    public GameObject InfoPanelPrefab;
    public float timeThreshold = 1f;

    private GameObject actMenu;
    private MonoBehaviour[] scripts;
    private bool actMenuEnabled = false;
    private List<string> actionNames;
    private float hoverTime;
    private bool createNewObject;
    private GameObject InfoPanel = null;

    private void Start()
    {
        scripts = GameObject.Find("Look_Camera").GetComponents<MonoBehaviour>();
        actMenu = GameObject.Find("ActionsMenu");
    }
    private void Update()
    {
        if (createNewObject && InfoPanel is null)
        {
           InfoPanel = Instantiate(InfoPanelPrefab, GameObject.Find("cnvs_HUD").transform);
            InfoPanel.transform.position = Input.mousePosition;
           for(int i = 0; i < InfoPanel.transform.childCount; i++) {
                var child = InfoPanel.transform.GetChild(i);
                switch (child.name) {
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
                    CreateActMenu();
                }
            }
            else
            {
                DestroyActMenu();
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
			Button button = buttonObject.GetComponent<Button>();

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
