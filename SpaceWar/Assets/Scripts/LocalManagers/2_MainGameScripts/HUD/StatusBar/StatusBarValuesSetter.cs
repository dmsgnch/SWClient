using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Resourses.MainGame;
using TMPro;

public class StatusBarValuesSetter : MonoBehaviour
{
    public TMP_Text resourcesText;
    public TMP_Text soldiersText;
    public TMP_Text researchShipsText;
    public TMP_Text colonizationShipsText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        resourcesText.text = HUD_values.totalNumResourses.ToString();
        soldiersText.text = HUD_values.totalNumSoldiers.ToString();
        researchShipsText.text = HUD_values.totalNumResearchShips.ToString();
        colonizationShipsText.text = HUD_values.totalNumColonizationShips.ToString();
    }
}
