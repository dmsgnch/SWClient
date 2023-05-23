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
	public class LobbyView : AbstractScreen<LobbyViewModel>
	{
		[SerializeField] private InputField emailInput;
		[SerializeField] private InputField passwordInput;
		[SerializeField] private UnityEngine.UI.Button loginButton;
		[SerializeField] private UnityEngine.UI.Button toRegister;

		private LobbyViewModel _lobbyViewModel;

		private void Awake()
		{
			loginButton.onClick.AddListener(_lobbyViewModel.OnLoginButtonClick);
			toRegister.onClick.AddListener(_lobbyViewModel.OnToRegisterButtonClick);
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				//TODO: Add confirm window			 

				if (Debug.isDebugBuild)
				{
					Debug.Log("Application quiting");
				}
				else
				{
					Application.Quit();
				}
			}
		}

		protected override void OnBind(LobbyViewModel lobbyViewModel)
		{
			_lobbyViewModel = lobbyViewModel;
		}
	}
}