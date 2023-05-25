using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Assets.Resourses.MainGame;
using System.Resources;

public class OnMouseTextsValuesSetter : MonoBehaviour
{
    public TMP_Text totalResourcesText;
    public TMP_Text totalSoldiersText;
    public TMP_Text usedSoldiersText;
    public TMP_Text totalResearchShipsText;
    public TMP_Text usedResearchShipsText;
    public TMP_Text totalColonizationShipsText;
    public TMP_Text usedColonizationShipsText;
    void Start()
    {
        
    }

    void Update()
    {
        totalResourcesText.text = HUD_values.totalNumResourses.ToString();
        totalSoldiersText.text = HUD_values.totalNumSoldiers.ToString();
        usedSoldiersText.text = HUD_values.usedNumSoldiers.ToString();
        totalResearchShipsText.text = HUD_values.totalNumResearchShips.ToString();
        usedResearchShipsText.text = HUD_values.usedNumResearchShips.ToString();
        totalColonizationShipsText.text = HUD_values.totalNumColonizationShips.ToString();
        usedColonizationShipsText.text = HUD_values.usedNumColonizationShips.ToString();
    }
}

