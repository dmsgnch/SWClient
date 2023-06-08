using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using ViewModels.Abstract;
using View.Abstract;
using Assets.View.Abstract;
using Assets.Scripts.ViewModels;
using Unity.VisualScripting;
using UnityEngine.UIElements;
namespace Assets.Scripts.View
{
	public class MenuView : AbstractScreen<MenuViewModel>
	{
		[SerializeField] private UnityEngine.UI.Button btn_Continue;
		[SerializeField] private UnityEngine.UI.Button btn_QuitApplication;
		[SerializeField] private UnityEngine.UI.Button btn_LeaveTheGame;
		[SerializeField] private GameObject confirmationPanel;

		private MenuViewModel _menuViewModel;

		#region Unity methods

		private void Awake()
		{
			btn_Continue.onClick.AddListener(bt_Continue_Click);
			btn_LeaveTheGame.onClick.AddListener(bt_LeaveTheGame_Click);
			btn_QuitApplication.onClick.AddListener(bt_QuitApplication_Click);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				bt_Continue_Click();
		}

		#endregion

		#region Buttons handlers

		void bt_Continue_Click()
		{
			PlayButtonClickSound();

			_menuViewModel.ContinueGame();
		}

		void bt_LeaveTheGame_Click()
		{
			PlayButtonClickSound();

			_menuViewModel.LeaveTheGame(this, confirmationPanel);
		}

		void bt_QuitApplication_Click()
		{
			PlayButtonClickSound();

			_menuViewModel.CloseApplication(this, confirmationPanel);
		}

		#endregion

		protected override void OnBind(MenuViewModel menuViewModel)
		{
			_menuViewModel = menuViewModel;
		}
	}
}

