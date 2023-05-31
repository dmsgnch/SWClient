using Assets.Scripts.ViewModels;
using Assets.View.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{
	public class HUDView : AbstractScreen<HUDViewModel>
	{
		[SerializeField] private GameObject pnl_onMouseTexts;
		[SerializeField] private GameObject pnl_Turn;
		[SerializeField] private GameObject pnl_StatusBar;

		private HUDViewModel _hudViewModel;
		//private OnMouseTextsValuesSetter

        private void Awake()
		{
			//var pnl_onMouseTextsScript = pnl_onMouseTexts.AddComponent<OnMouseTextsValuesSetter>();
			//_hudViewModel.UpdateStatusBar(ref pnl_StatusBar);
		}

		public void UpdateSession()
		{
			_hudViewModel.GetSessionRequestCreate();
		}

		public void UpdateHero()
		{
			_hudViewModel.GetHeroRequestCreate();
		}

		private void OnEnable()
		{
			if (_hudViewModel is null) return;

			UpdateSession();
		}

		protected override void OnBind(HUDViewModel hudViewModel)
		{
			_hudViewModel = hudViewModel;
		}
	}
}
