using System;
using View.Abstract;
using ViewModels.Abstract;
using UnityEngine;

namespace Assets.View.Abstract
{
	public abstract class AbstractScreen<TModel> : BaseScreen
		where TModel : IViewModel
	{
		[SerializeField] private AudioClip ButtonClick;

		internal void PlayButtonClickSound()
		{
			AudioSystem audioSystem = GetSceneComponent<AudioSystem>();

			audioSystem.PlaySound(ButtonClick);
		}

		private T GetSceneComponent<T>() where T : Component
		{
			return FindFirstObjectByType<T>() ?? throw new Exception();
		}

		public override Type ModelType => typeof(TModel);
		protected TModel _model;

		public override void Show()
		{
			gameObject.SetActive(true);
		}

		public override void Close()
		{
			gameObject.SetActive(false);
		}

		public override void Bind(object model)
		{
			if (model is TModel)
				Bind((TModel)model);
		}

		public void Bind(TModel model)
		{
			_model = model;
			OnBind(model);
		}

		protected abstract void OnBind(TModel model);
	}
}
