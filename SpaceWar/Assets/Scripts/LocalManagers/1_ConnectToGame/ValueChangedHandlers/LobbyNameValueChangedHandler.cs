using Assets.Scripts.Components;
using Assets.Scripts.Managers;
using Scripts.RegisterLoginScripts;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LocalManagers.ConnectToGame.ValueChangedHandlers
{

    /// <summary>
    /// script that defines behavior on changing value 
    /// in lobby to create name input box 
    /// </summary>
    public class LobbyNameValueChangedHandler : InputValueChangedHandlerBase<LobbyNameValueChangedHandler>
    {
		public bool IsValidated { get; set; } = false;

		/// <summary>
		/// function that should be called when lobby to create box value is changed
		/// </summary>
		/// <param name="value">new value for lobby name</param>
		public override void OnValueChanged(string value)
        {
            _icon.color = BaseColor;

			DataValidator dataValidator = new DataValidator();

			if (dataValidator.ValidateString(value, out string result))
            {
				GameManager.Instance.MainDataStore.LobbyToCreateName = value;

                _icon.sprite = _validSprite;

				IsValidated = true;	

			}
            else
            {
				Debug.LogError($"{result}");

				IsValidated = false;
				
				_icon.sprite = _errorSprite;
            }
        }
    }
}