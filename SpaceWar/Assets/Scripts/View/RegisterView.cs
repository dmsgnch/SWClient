using Assets.Scripts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using Assets.View.Abstract;
using Assets.Scripts.Components;

namespace Assets.Scripts.View
{
	public class RegisterView : AbstractScreen<RegisterViewModel>
	{
		[SerializeField] private InputField usernameInput;
		[SerializeField] private InputField emailInput;
		[SerializeField] private InputField passwordInput;
		[SerializeField] private InputField confirmPasswordInput;
		[SerializeField] private Button registerButton;
		[SerializeField] private Button toLogin;

		[SerializeField] private GameObject confirmationPrefab;

		private RegisterViewModel _registerViewModel;

		#region Unity methods

		private void Awake()
		{
			registerButton.onClick.AddListener(OnRegisterButtonClick);
			toLogin.onClick.AddListener(OnToLoginButtonClick);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{ 
				_registerViewModel.CloseApplication(this, confirmationPrefab);
			}
		}

		#endregion

		#region Button handlers

		private void OnRegisterButtonClick()
		{
			PlayButtonClickSound();

			if (_registerViewModel.ValidateName(usernameInput.text) &&
				_registerViewModel.ValidateEmail(emailInput.text) &&
				_registerViewModel.ValidatePassword(passwordInput.text, confirmPasswordInput.text))
			{
				_registerViewModel.SendRegisterRequest(usernameInput.text, emailInput.text, passwordInput.text);
			}
		}

		private void OnToLoginButtonClick()
		{
			PlayButtonClickSound();

			_registerViewModel.ToLogin();
		}

		#endregion

		protected override void OnBind(RegisterViewModel registerViewModel)
		{
			_registerViewModel = registerViewModel;
		}
	}
}
