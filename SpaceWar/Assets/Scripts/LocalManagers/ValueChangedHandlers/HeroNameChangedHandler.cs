using Scripts.RegisterLoginScripts;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LocalManagers.ConnectToGame.ValueChangedHandlers
{
    /// <summary>
    /// script that defines behavior on changing value 
    /// in hero name input box 
    /// </summary>
    public class HeroNameChangedHandler : InputValueChangedHandler
    {
        /// <summary>
        /// function that should be called when hero name box value is changed
        /// </summary>
        /// <param name="value">new value for hero name</param>
        public override void OnValueChanged(string value)
        {
            _icon.color = new Color(205,205,205,255);
            if (InputValidator.Validate(value))
            {
                NetworkingManager.Instance.HeroName = value;
                Debug.Log($"hero name changed to {NetworkingManager.Instance.HeroName}");
                _icon.sprite = _validSprite;
            }
            else
            {
                _icon.sprite = _errorSprite;
            }
        }
    }
}