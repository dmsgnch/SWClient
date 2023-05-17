using Assets.Resourses.MainGame;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnPanelValuesSetter : MonoBehaviour
{
    public TMP_Text TimeLeftText;
    public TMP_Text currentTurnHeroNameText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeLeftText.text = HUD_values.timeLeft.ToString();
        currentTurnHeroNameText.text = HUD_values.currentTurnHeroName;
    }
}
