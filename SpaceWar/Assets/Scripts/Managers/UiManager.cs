using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using View.Abstract;
using ViewModels.Abstract;

public class UiManager : MonoBehaviour
{
	private IEnumerable<BaseScreen> _screens;
	private Dictionary<Type, BaseScreen> _screensMap;
	private Dictionary<Type, BaseScreen> _shownScreens;

	public void Init(IEnumerable<BaseScreen> screens)
	{
		foreach (var screen in _screens)
		{
			screen.gameObject.SetActive(false);
		}
		_screensMap = _screens.ToDictionary(e => e.ModelType, e => e);
	}

	public void BindAndShow<TModel>(TModel model) where TModel : IViewModel
	{
		if (_screensMap.TryGetValue(typeof(TModel), out var screen))
		{
			screen.Bind(model);
			screen.Show();
			_shownScreens.Add(typeof(TModel), screen);
		}
	}

	public void Hide<TModel>() where TModel : IViewModel
	{
		if (_shownScreens.TryGetValue(typeof(TModel), out var screen))
		{
			screen.Dispose();
			screen.Close();
			_shownScreens.Remove(typeof(TModel));
		}
	}
}