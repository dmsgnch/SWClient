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
        switch ((PlanetStatus)planet.Status) {
            case PlanetStatus.Colonized:
                actionNames = new List<string> { };
                return;
               // break;
            case PlanetStatus.Known:
                actionNames = new List<string> {"Research"};
                break;
            case PlanetStatus.Researched:
                actionNames = new List<string> { "Colonize" };
                break;
            case PlanetStatus.Colonizing:
                actionNames = new List<string> { };
                return;
              //  break;
            case PlanetStatus.Enemy:
                actionNames = new List<string> { "Attack" };
                break;
            case PlanetStatus.Unknown:
                actionNames = new List<string> { };
                return;
              //  break;
            case PlanetStatus.HasStation:
                actionNames = new List<string> { };
                return;
               // break;
            default:
                throw new DataException();
        }
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
