using SharedLibrary.Models;
using SharedLibrary.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace Components
{
	public static class ColorParser
	{

		private static Dictionary<ColorStatus, Color> Colors { get; set; } =
			new Dictionary<ColorStatus, Color>()
			{
				{ ColorStatus.Red, Color.red },
				{ ColorStatus.Blue, Color.blue },
				{ ColorStatus.Yellow, Color.yellow },
			};


		public static Color GetColor(ColorStatus colorStatus)
		{
			if (Colors.TryGetValue(colorStatus, out Color value))
			{
				return value;
			}
			else
			{
				throw new ArgumentException();
			}
		}

		public static ColorStatus GetColorStatus(Color color)
		{
			return Colors.FirstOrDefault(x => x.Value.Equals(color)).Key;
		}

		public static ColorStatus GetNextColor(ColorStatus actualColorStatus)
		{
			var actualIndex = GetColorStatusIndex(actualColorStatus);

			return (ColorStatus)(actualIndex + 1 != Colors.Count ? actualIndex + 1 : 0);
		}

		public static ColorStatus GetPreviousColor(ColorStatus actualColorStatus)
		{
			var actualIndex = GetColorStatusIndex(actualColorStatus);

			return (ColorStatus)(actualIndex != 0 ? actualIndex - 1 : Colors.Count - 1);
		}

		private static int GetColorStatusIndex(ColorStatus actualColorStatus)
		{
			return Colors.Keys.ToList().IndexOf(actualColorStatus);
		} 
	}
}