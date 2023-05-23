using Scripts.RegisterLoginScripts;
using UnityEngine;
using UnityEngine.UI;

namespace LocalManagers.ConnectToGame.ValueChangedHandlers
{
    /// <summary>
    /// abstract class that defines behavior on changing values of input boxes 
    /// </summary>
    public abstract class InputValueChangedHandlerBase<T> : ComponentSingleton<T> where T : Component
    {
        [SerializeField] protected Image _icon;
        [SerializeField] protected Sprite _validSprite;
        [SerializeField] protected Sprite _errorSprite;

        protected Color BaseColor { get; set; } = new Color(205, 205, 205, 255);

		/// <summary>
		/// function that should be called when input box value is changed
		/// </summary>
		/// <param name="value">new value of input box</param>
		public abstract void OnValueChanged(string value);
    }
}