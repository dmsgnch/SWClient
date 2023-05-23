using Assets.Scripts.Components;
using Assets.Scripts.Components.Abstract;
using Assets.Scripts.Managers;
using LocalManagers.RegisterLoginRequests;
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
		CreateLoginRequest createLoginRequest;

		public LoginViewModel()
		{ }

		public void Login(string email, string password)
		{	
			createLoginRequest = new GameObject().AddComponent<CreateLoginRequest>();

			createLoginRequest.onCoroutineFinished.AddListener(OnCoroutineFinishedEventHandler);

			createLoginRequest.CreateRequest(email, password);
		}

		private void OnCoroutineFinishedEventHandler()
		{
			Destroy(createLoginRequest.gameObject);
		}

		public void OnToRegisterButtonClick()
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

			var validationResult = validator.ValidateEmail(email);

			if (string.IsNullOrWhiteSpace(validator.ValidateEmail(validationResult))) 
				return true;

			//TODO: Infopanel outputing	
			Debug.Log($"{validationResult}");
			return false;
		}

		public bool ValidatePassword(string password)
		{
			return true;

			//TODO: Implement validation
		}
	}
}
