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
	public class LoginView : AbstractScreen<LoginViewModel>
	{
		[SerializeField] private InputField emailInput;
		[SerializeField] private InputField passwordInput;
		[SerializeField] private InputField confirmPasswordInput;
		[SerializeField] private UnityEngine.UI.Button loginButton;
		[SerializeField] private UnityEngine.UI.Button toRegister;

		private LoginViewModel _loginViewModel;

		private void Awake()
		{
			loginButton.onClick.AddListener(OnLoginButtonClick);
			toRegister.onClick.AddListener(OnToRegisterButtonClick);
		}

		private void OnLoginButtonClick()
		{
			if (_loginViewModel.ValidateEmail(emailInput.text) &&
			_loginViewModel.ValidatePassword(passwordInput.text))
			{
				_loginViewModel.Login(emailInput.text, passwordInput.text);
			}			
		}

		private void OnToRegisterButtonClick()
		{
			_loginViewModel.ToRegister();
		}

		protected override void OnBind(LoginViewModel loginViewModel)
		{
			_loginViewModel = loginViewModel;
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				//TODO: Add confirm window			 

				_loginViewModel.CloseApplication();
			}
		}		
	}
}