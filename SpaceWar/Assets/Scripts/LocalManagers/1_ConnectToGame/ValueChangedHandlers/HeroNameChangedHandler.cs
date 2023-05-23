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
    public class HeroNameChangedHandler : InputValueChangedHandlerBase
    {
        [SerializeField] private Button ConnectToGameButton;
		[SerializeField] private Button StartNewGameButton;

		public void Start()
		{
			ConnectToGameButton.interactable = false;
			StartNewGameButton.interactable = false;
		}

		/// <summary>
		/// function that should be called when hero name box value is changed
		/// </summary>
		/// <param name="value">new value for hero name</param>
		public override void OnValueChanged(string value)
        {
            _icon.color = new Color(205,205,205,255);
            if (InputValidator.Validate(value))
            {
				GameManager.Instance.MainDataStore.HeroName = value;
                //Debug.Log($"hero name changed to {NetworkingManager.Instance.HeroName}");
                _icon.sprite = _validSprite;

                if (!string.IsNullOrWhiteSpace(GameManager.Instance.MainDataStore.SelectedLobbyId) &&
					InputValidator.Validate(GameManager.Instance.MainDataStore.SelectedLobbyId))                
					ConnectToGameButton.interactable = true;

				if (!string.IsNullOrWhiteSpace(GameManager.Instance.MainDataStore.LobbyToCreateName) &&
					InputValidator.Validate(GameManager.Instance.MainDataStore.LobbyToCreateName))
					StartNewGameButton.interactable = true;
			}
            else
            {
				ConnectToGameButton.interactable = false;
				StartNewGameButton.interactable = false;

				_icon.sprite = _errorSprite;
            }
        }
    }
}