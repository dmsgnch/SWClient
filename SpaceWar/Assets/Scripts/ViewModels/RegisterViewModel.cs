using Assets.Scripts.Components.Abstract;
using Assets.Scripts.Managers;
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

		CreateRegisterRequest createRegisterRequest;

		public RegisterViewModel()
		{ }		

		public void Register(string name, string email, string password)
		{
			createRegisterRequest = new GameObject().AddComponent<CreateRegisterRequest>();

			createRegisterRequest.CreateRequest(name, email, password);
		}

		public void ToLogin()
		{
			GameManager.Instance.ChangeState(GameState.Login);
		}

		private void OnCoroutineFinishedEventHandler()
		{
			Destroy(createRegisterRequest.gameObject);
		}

		public void CloseApplication()
		{
			if (Debug.isDebugBuild)
				Debug.Log("Application quiting");

			Application.Quit();
		}

		public bool ValidateName(string name)
		{
			return true;

			//TODO: Implement validation
		}

		public bool ValidateEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				//TODO: Infopanel outputing
				Debug.Log("Displaying information panel in the future. You must fill the all of filds");
				return false;
			}

			string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

			if (!Regex.IsMatch(email, pattern))
			{
				//TODO: Infopanel outputing
				Debug.Log("Displaying information panel in the future. Email is not match to the pattern");
				return false;
			}

			return true;
		}

		public bool ValidatePassword(string password, string confirmPassword)
		{
			return true;

			//TODO: Implement validation
		}
	}
}
