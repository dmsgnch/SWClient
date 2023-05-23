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
    public class LobbyNameValueChanged : InputValueChangedHandlerBase
    {
        [SerializeField] private Button StartNewGameButton;
		/// <summary>
		/// function that should be called when lobby to create box value is changed
		/// </summary>
		/// <param name="value">new value for lobby name</param>
		public override void OnValueChanged(string value)
        {
            _icon.color = new Color(205, 205, 205, 255);
            if (InputValidator.Validate(value))
            {
				GameManager.Instance.MainDataStore.LobbyToCreateName = value;
                //Debug.Log($"lobby to create name changed to {NetworkingManager.Instance.LobbyToCreateName}");
                _icon.sprite = _validSprite;

				if (!string.IsNullOrWhiteSpace(GameManager.Instance.MainDataStore.HeroName) &&
					InputValidator.Validate(GameManager.Instance.MainDataStore.HeroName))				
					StartNewGameButton.interactable = true;			

			}
            else
            {
				StartNewGameButton.interactable = false;
				_icon.sprite = _errorSprite;
            }
        }
    }
}