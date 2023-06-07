using Assets.Scripts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using Assets.View.Abstract;
using Unity.VisualScripting;

namespace Assets.Scripts.View
{
	public class FPSView : AbstractScreen<FPSViewModel>
	{	private FPSViewModel _fpsViewModel;

		[SerializeField] private Text _text;

		#region Unity methods

		void Awake()
		{
			if (_fpsViewModel is not null)
			{
				_fpsViewModel.CacheStringsAndCreateArray();
			}
			
		}
		void Update()
		{
			if (_fpsViewModel is not null)
			{
				_fpsViewModel.UpdateValue();
			}
		}

		#endregion

		#region Commands

		private void SetTextField()
		{
			_fpsViewModel.Text = _text;
		}

		#endregion

		protected override void OnBind(FPSViewModel fpsViewModel)
		{
			_fpsViewModel = fpsViewModel;
			SetTextField();
		}
	}
}
