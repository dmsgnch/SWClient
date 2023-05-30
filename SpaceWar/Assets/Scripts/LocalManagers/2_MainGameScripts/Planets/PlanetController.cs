using Assets.Resourses.MainGame;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlanetController : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public GameObject dropdownPrefab;
    public GameObject planetPrefab;

    private GameObject actMenu;
    private MonoBehaviour[] scripts;
    private bool actMenuEnabled = false;


    private void Start()
    {
        scripts = GameObject.Find("Look_Camera").GetComponents<MonoBehaviour>();
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
        actMenu = Instantiate(dropdownPrefab);
        actMenu.name = "actMenu_" + transform.name;
        actMenu.transform.position = Input.mousePosition;
        actMenu.transform.SetParent(GameObject.Find("cnvs_HUD").transform);
        actMenu.transform.Find("bt_research").GetComponent<Button>().onClick.AddListener(onResearchClick);
        actMenu.transform.Find("bt_colonize").GetComponent<Button>().onClick.AddListener(onColonizeClick);
        actMenuEnabled = true;
        DisableScripts();
    }

    void DestroyActMenu()
    {
        Destroy(actMenu);
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

    void onResearchClick()
    {
        Debug.Log($"Button \"research\" clicked on planet \"{transform.name}\"");
        DestroyActMenu();
    }
    void onColonizeClick()
    {
        Debug.Log($"Button \"colonize\" clicked on planet \"{transform.name}\"");
        DestroyActMenu();
    }

}
