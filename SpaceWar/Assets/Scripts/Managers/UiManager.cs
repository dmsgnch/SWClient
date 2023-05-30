using Assets.View.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using View.Abstract;
using ViewModels.Abstract;

public class UiManager : ComponentSingleton<UiManager>
{
	private Dictionary<Type, BaseScreen> _screensMap;
	private Dictionary<Type, BaseScreen> _shownScreens = new Dictionary<Type, BaseScreen>();

	public void Init(IEnumerable<BaseScreen> screens)
	{
		foreach (var screen in screens)
		{
			screen.gameObject.SetActive(false);
		}
		_screensMap = screens.ToDictionary(e => e.ModelType, e => e);
	}

	public void BindAndShow<TViewModel>(TViewModel model)
		where TViewModel : IViewModel
	{
		if (_shownScreens.ContainsKey(typeof(TViewModel))) return;

		if (_screensMap.TryGetValue(typeof(TViewModel), out var screen))
		{
			screen.Bind(model);
			screen.Show();
			_shownScreens.Add(typeof(TViewModel), screen);
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