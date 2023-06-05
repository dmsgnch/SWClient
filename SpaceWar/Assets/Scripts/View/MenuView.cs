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
		[SerializeField] private UnityEngine.UI.Button btn_SaveGame;
		[SerializeField] private UnityEngine.UI.Button btn_LoadGame;
		[SerializeField] private UnityEngine.UI.Button btn_Settings;
		[SerializeField] private UnityEngine.UI.Button btn_QuitApplication;
		[SerializeField] private UnityEngine.UI.Button btn_LeaveTheGame;
        [SerializeField] private GameObject confirmationPanel;

        private MenuViewModel _menuViewModel;

		private void Awake()
		{
			btn_Continue.onClick.AddListener(bt_Continue_Click);
			btn_SaveGame.onClick.AddListener(bt_SaveGame_Click);
			btn_LoadGame.onClick.AddListener(bt_LoadGame_Click);
			btn_Settings.onClick.AddListener(bt_Settings_Click);
			btn_LeaveTheGame.onClick.AddListener(bt_LeaveTheGame_Click);
			btn_QuitApplication.onClick.AddListener(bt_QuitApplication_Click);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				bt_Continue_Click();
		}

		void bt_Continue_Click() => _menuViewModel.ContinueGame();

		void bt_SaveGame_Click() => _menuViewModel.SaveGame();

		void bt_LoadGame_Click() => _menuViewModel.LoadGame();

		void bt_Settings_Click() => _menuViewModel.Settings();

		async void bt_LeaveTheGame_Click() => await _menuViewModel.LeaveTheGame();

		void bt_QuitApplication_Click() => _menuViewModel.CloseApplication(confirmationPanel,gameObject);

		protected override void OnBind(MenuViewModel menuViewModel)
		{
			_menuViewModel = menuViewModel;
		}
	}
}

