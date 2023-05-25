using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LocalManagers.ConnectToGame
{
	class ColorChanger : MonoBehaviour, IPointerClickHandler
	{
		private Button button;
		private readonly Color[] colors = new[] { Color.blue, Color.red, Color.yellow };

		public void SetButton(Button button)
		{
			this.button = button;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
				NextColor();
			else if (eventData.button == PointerEventData.InputButton.Right)
				PreviousColor();
		}

		private void NextColor()
		{
			var image = button.image;
			if (!colors.Contains(image.color)) return;

			var currentColor = image.color;
			Color nextColor;
			var index = Array.IndexOf(colors, currentColor);
			if (index != colors.Length - 1) nextColor = colors[index + 1];
			else nextColor = colors.First();

			image.color = nextColor;
		}

		private void PreviousColor()
		{
			var image = button.image;
			if (!colors.Contains(image.color)) return;

			var currentColor = image.color;
			Color previousColor;
			var index = Array.IndexOf(colors, currentColor);
			if (index != 0) previousColor = colors[index - 1];
			else previousColor = colors.Last();

			image.color = previousColor;
		}
	}
}