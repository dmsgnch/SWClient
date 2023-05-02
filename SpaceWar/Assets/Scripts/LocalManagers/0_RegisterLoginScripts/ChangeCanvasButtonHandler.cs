using System.Collections;
using System.Collections.Generic;
using Components;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCanvasButtonHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick() => 
        ChangeActiveObjects.Instance.SwapActivity("cnvs_register", "cnvs_login");
}
