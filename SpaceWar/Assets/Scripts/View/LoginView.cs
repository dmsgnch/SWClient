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
using Button = UnityEngine.UI.Button;

namespace Assets.Scripts.View
{
	public class LoginView : AbstractScreen<LoginViewModel>
	{
		[SerializeField] private InputField emailInput;
		[SerializeField] private InputField passwordInput;
		[SerializeField] private Button loginButton;
		[SerializeField] private Button toRegister;
		[SerializeField] private GameObject confirmationPrefab;

		private LoginViewModel _loginViewModel;

		#region Unity methods

		private void Awake()
		{
			loginButton.onClick.AddListener(OnLoginButtonClick);
			toRegister.onClick.AddListener(OnToRegisterButtonClick);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{	
				_loginViewModel.CloseApplication(this, confirmationPrefab);
			}
		}

		#endregion

		#region Buttons handlers

		private void OnLoginButtonClick()
		{
			PlayButtonClickSound();

			if (_loginViewModel.ValidateEmail(emailInput.text) &&
			_loginViewModel.ValidatePassword(passwordInput.text))
			{
				_loginViewModel.SendLoginRequest(emailInput.text, passwordInput.text);
			}			
		}

		private void OnToRegisterButtonClick()
		{
			PlayButtonClickSound();

			_loginViewModel.ToRegister();
		}

		#endregion

		protected override void OnBind(LoginViewModel loginViewModel)
		{
			_loginViewModel = loginViewModel;
		}		
	}
}