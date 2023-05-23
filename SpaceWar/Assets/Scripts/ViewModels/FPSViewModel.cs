using Assets.Scripts.Components.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ViewModels.Abstract;

namespace Assets.Scripts.ViewModels
{
	public class FPSViewModel : ViewModelBase
	{
		public Text Text { get; set; }

		private Dictionary<int, string> CachedNumberStrings = new();
		private int[] _frameRateSamples;
		private int _cacheNumbersAmount = 300;
		private int _averageFromAmount = 30;
		private int _averageCounter = 0;
		private int _currentAveraged;

		public void CacheStringsAndCreateArray()
		{
			// Cache strings and create array
			{
				for (int i = 0; i < _cacheNumbersAmount; i++)
				{
					CachedNumberStrings[i] = i.ToString();
				}
				_frameRateSamples = new int[_averageFromAmount];
			}
		}
		public void UpdateValue()
		{
			if (_frameRateSamples is null) CacheStringsAndCreateArray();

			// Sample
			{
				var currentFrame = (int)Math.Round(1f / Time.smoothDeltaTime); // If your game modifies Time.timeScale, use unscaledDeltaTime and smooth manually (or not).
				_frameRateSamples[_averageCounter] = currentFrame;
			}

			// Average
			{
				var average = 0f;

				foreach (var frameRate in _frameRateSamples)
				{
					average += frameRate;
				}

				_currentAveraged = (int)Math.Round(average / _averageFromAmount);
				_averageCounter = (_averageCounter + 1) % _averageFromAmount;
			}

			// Assign to UI
			{
				Text.text = _currentAveraged < _cacheNumbersAmount && _currentAveraged > 0
					? CachedNumberStrings[_currentAveraged]
					: _currentAveraged < 0
						? "< 0"
						: _currentAveraged > _cacheNumbersAmount
							? $"> {_cacheNumbersAmount}"
							: "-1";
			}
		}
	}
}
