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
    public class MenuView : AbstractScreen <MenuViewModel>
    {
        [SerializeField] private UnityEngine.UI.Button btn_Continue;
        [SerializeField] private UnityEngine.UI.Button btn_SaveGame;
        [SerializeField] private UnityEngine.UI.Button btn_LoadGame;
        [SerializeField] private UnityEngine.UI.Button btn_Settings;
        [SerializeField] private UnityEngine.UI.Button btn_QuitTheGame;
        private MenuViewModel _menuViewModel;
        // Start is called before the first frame update
        private void Awake()
        {
            btn_Continue.onClick.AddListener(btn_Continue_Click);
            btn_SaveGame.onClick.AddListener(btn_SaveGame_Click);
            btn_LoadGame.onClick.AddListener (btn_LoadGame_Click);
            btn_Settings.onClick.AddListener(btn_Settings_Click);
            btn_QuitTheGame.onClick.AddListener(btn_QuitTheGame_Click);
        }
        void btn_Continue_Click()
        {
            _menuViewModel.ContinueGame();
        }
        void btn_SaveGame_Click()
        {
            _menuViewModel.SaveGame();
        }
        void btn_LoadGame_Click()
        {
            _menuViewModel.LoadGame();
        }
        void btn_Settings_Click()
        {
            _menuViewModel.Settings();
        }
        void btn_QuitTheGame_Click() 
        {
            _menuViewModel.QuitTheGame();
        }
        protected override void OnBind(MenuViewModel menuViewModel)
        {
            _menuViewModel = menuViewModel;
        }

    }
}

