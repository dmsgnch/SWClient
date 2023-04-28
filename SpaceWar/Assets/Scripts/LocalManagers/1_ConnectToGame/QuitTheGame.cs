using System;
using UnityEngine;
using UnityEngine.UI;

namespace LocalManagers.ConnectToGame
{
    /// <summary> If you click on the button, this class initiates an exit from the application </summary>
    public class QuitTheGame : MonoBehaviour
    {
        private void Start()
        {
            Button btn = gameObject.GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        void OnClick() => Application.Quit();
    }
}