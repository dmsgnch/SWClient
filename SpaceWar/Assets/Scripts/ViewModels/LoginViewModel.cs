using Assets.Scripts.Components;
using Assets.Scripts.Managers;
using Components.Abstract;
using Components;
using LocalManagers.RegisterLoginRequests;
using SharedLibrary.Responses.Abstract;
using SharedLibrary.Responses;
using UnityEngine;
using ViewModels.Abstract;

namespace Assets.Scripts.ViewModels
{
	public class LoginViewModel : ViewModelBase
	{

		public static GameObject CreateLoginRequestObject { get; set; }

		public LoginViewModel()
		{ }

		public void Login(string email, string password)
		{
			CreateLoginRequestObject = new GameObject("LoginRequest");

			var createLoginRequest = CreateLoginRequestObject.AddComponent<CreateLoginRequest>();

			createLoginRequest.CreateRequest(email, password);			
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

			bool validationResult = validator.ValidateEmail(email, out string message);
			if (!string.IsNullOrEmpty(message)) ShowError(message);
			return validationResult;
		}

		public bool ValidatePassword(string password)
		{
			DataValidator validator = new DataValidator();

			bool validationResult = validator.ValidatePassword(password, out string message);
			if (!string.IsNullOrEmpty(message)) ShowError(message);
			return validationResult;
		}

		private void ShowError(string message)
        {
			InformationPanelController.Instance.CreateMessage(
					InformationPanelController.MessageType.ERROR, message);
		}		
	}
}
