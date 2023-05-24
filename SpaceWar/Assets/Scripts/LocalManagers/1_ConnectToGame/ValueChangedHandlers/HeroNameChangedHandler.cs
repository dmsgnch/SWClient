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
    /// in hero name input box 
    /// </summary>
    public class HeroNameChangedHandler : InputValueChangedHandlerBase<HeroNameChangedHandler>
    {
		public bool IsValidated { get; set; } = false;

		/// <summary>
		/// function that should be called when hero name box value is changed
		/// </summary>
		/// <param name="value">new value for hero name</param>
		public override void OnValueChanged(string value)
        {
			_icon.color = BaseColor;

			DataValidator dataValidator = new DataValidator();

			if (dataValidator.ValidateString(value))
            {
				GameManager.Instance.ConnectToGameDataStore.HeroName = value;
               
                _icon.sprite = _validSprite;

				IsValidated = true;
			}
            else
            {
				IsValidated = false;

				_icon.sprite = _errorSprite;
            }
        }
    }
}