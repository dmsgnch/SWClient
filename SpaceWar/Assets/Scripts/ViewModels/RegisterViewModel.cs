using Assets.Scripts.Components;
using Assets.Scripts.Components.Abstract;
using Assets.Scripts.Managers;
using Components.Abstract;
using LocalManagers.RegisterLoginRequests;
using Scripts.RegisterLoginScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using ViewModels.Abstract;

namespace Assets.Scripts.ViewModels
{
	public class RegisterViewModel : ViewModelBase
	{

		public static CreateRegisterRequest CreateRegisterRequest { get; set; }

		public RegisterViewModel()
		{ }

		public void Register(string name, string email, string password)
		{
			CreateRegisterRequest = new GameObject("ReisterRequest").AddComponent<CreateRegisterRequest>();

			CreateRegisterRequest.CreateRequest(name, email, password);
		}

		public void ToLogin()
		{
			GameManager.Instance.ChangeState(GameState.Login);
		}

		public void CloseApplication()
		{
			if (Debug.isDebugBuild)
				Debug.Log("Application quiting");

			Application.Quit();
		}

		public bool ValidateName(string name)
		{
			DataValidator validator = new DataValidator();

			return validator.ValidateString(name);
		}

		public bool ValidateEmail(string email)
		{
			DataValidator validator = new DataValidator();

			return validator.ValidateEmail(email);
		}

		public bool ValidatePassword(string password, string confirmPassword)
		{
			DataValidator validator = new DataValidator();

			return validator.ValidatePassword(password,confirmPassword);
		}

		public class RegisterResponseHandler : IResponseHandler
		{
			public void OnRequestFinished()
			{
				Destroy(CreateRegisterRequest.gameObject);
			}
		}
	}

}
