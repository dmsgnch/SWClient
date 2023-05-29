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
		[SerializeField] private GameObject playerList;
		[SerializeField] private GameObject playerListItemPrefab;
		[SerializeField] private Button startButton;
		[SerializeField] private GameObject readyButton;

		private HUDViewModel _hudViewModel;

		private void Awake()
		{
			startButton.onClick.AddListener(OnStartButtonClick);

		}

		public void OnStartButtonClick()
		{
			_hudViewModel.CreateMessage();
			//Reference on view model
		}

		private void OnEnable()
		{
			if (_hudViewModel is null) return;

			//Functionality
		}

		protected override void OnBind(HUDViewModel hudViewModel)
		{
			_hudViewModel = hudViewModel;
		}
	}
}
