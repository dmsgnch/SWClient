using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.Abstract;
using ViewModels.Abstract;

namespace Assets.View.Abstract
{
	public abstract class AbstractScreen<TModel> : BaseScreen 
		where TModel : IViewModel
	{
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
