using Assets.Scripts.Components;
using Assets.Scripts.Components.Abstract;
using Assets.Scripts.Managers;
using Components.Abstract;
using Components;
using LocalManagers.RegisterLoginRequests;
using SharedLibrary.Responses.Abstract;
using SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using ViewModels.Abstract;

namespace Assets.Scripts.ViewModels
{
	public class LoginViewModel : ViewModelBase
	{

		public static CreateLoginRequest CreateLoginRequest { get; set; }

		public LoginViewModel()
		{ }

		public void Login(string email, string password)
		{
			CreateLoginRequest = new GameObject("LoginRequest").AddComponent<CreateLoginRequest>();

			CreateLoginRequest.CreateRequest(email, password);			
		}

		public void ToRegister()
		{
			GameManager.Instance.ChangeState(GameState.Register);
		}

		public void CloseApplication()
		{
			if (Debug.isDebugBuild)
				Debug.Log("Application quiting");

			Application.Quit();
		}

		public bool ValidateEmail(string email)
		{
			DataValidator validator = new DataValidator();

			string validationResult; 
			if (validator.ValidateEmail(email, out validationResult))
			{
				Debug.Log($"Success email validation");
				return true;
			}

			//TODO: Infopanel outputing	
			Debug.Log($"{validationResult}");
			return false;
		}

		public bool ValidatePassword(string password)
		{
			Debug.Log($"Mock password validation");

			return true;

			//TODO: Implement validation
		}

		public class LoginResponseHandler : IResponseHandler
		{
			public void BodyConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
				where T : ResponseBase
			{
				//Not create information panel
			}

			public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
				where T : ResponseBase
			{
				GameManager.Instance.MainDataStore.AccessToken =
					requestForm.GetResponseResult<AuthenticationResponse>().Token;

				GameManager.Instance.ChangeState(GameState.LoadConnectToGameScene);				
			}

			public void OnRequestFinished()
			{
				Destroy(CreateLoginRequest.gameObject);
			}
		}
	}
}
