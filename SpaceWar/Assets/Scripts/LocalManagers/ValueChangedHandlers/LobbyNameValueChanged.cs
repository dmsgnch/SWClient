using Scripts.RegisterLoginScripts;
using System.Collections;
using UnityEngine;

namespace LocalManagers.ConnectToGame.ValueChangedHandlers
{
    /// <summary>
    /// script that defines behavior on changing value 
    /// in lobby to create name input box 
    /// </summary>
    public class LobbyNameValueChanged : InputValueChangedHandler
    {
        /// <summary>
        /// function that should be called when lobby to create box value is changed
        /// </summary>
        /// <param name="value">new value for lobby name</param>
        public override void OnValueChanged(string value)
        {
            _icon.color = new Color(205, 205, 205, 255);
            if (InputValidator.Validate(value))
            {
                NetworkingManager.Instance.LobbyToCreateName = value;
                Debug.Log($"lobby to create name changed to {NetworkingManager.Instance.LobbyToCreateName}");
                _icon.sprite = _validSprite;
            }
            else
            {
                _icon.sprite = _errorSprite;
            }
        }
    }
}