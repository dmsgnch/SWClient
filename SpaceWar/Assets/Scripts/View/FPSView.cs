using Assets.Scripts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using Assets.View.Abstract;

namespace Assets.Scripts.View
{
	public class FPSView : AbstractScreen<FPSViewModel>
	{	private FPSViewModel _fpsViewModel;

		[SerializeField] private Text _text;

		private void SetTextField()
		{
			_fpsViewModel.Text = _text;
		}

		protected override void OnBind(FPSViewModel fpsViewModel)
		{
			_fpsViewModel = fpsViewModel;
			SetTextField();
		}
	}
}
