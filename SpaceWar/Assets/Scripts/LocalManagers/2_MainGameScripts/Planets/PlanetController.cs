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

public class PlanetController : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public Planet planet;
    public GameObject planetPrefab;
    public GameObject ButtonPrefab;

    private GameObject actMenu;
    private MonoBehaviour[] scripts;
    private bool actMenuEnabled = false;
    private List<string> actionNames;

    private void Start()
    {
        scripts = GameObject.Find("Look_Camera").GetComponents<MonoBehaviour>();
        actMenu = GameObject.Find("ActionsMenu");
    }
    private void Update()
    {
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
		switch ((PlanetStatus)planet.Status)
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
                //TODO: Add checking for availability
				return new List<string> { "Attack" };
			default:
				throw new DataException();
		}
	}

	private List<string> GetButtonsListByPlanetFortification() 
    {
		switch ((Fortification)planet.FortificationLevel)
		{
			case Fortification.None:
				return new List<string> { "Build low level fortifications (R)" };
			case Fortification.Weak:
				return new List<string> { "Build medium level fortifications (R)" };
			case Fortification.Reliable:
				return new List<string> { "Build total fortifications (R)" };
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

}
