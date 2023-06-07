using Assets.Scripts.Components;
using Assets.Scripts.Managers;
using Components;
using Components.Abstract;
using LocalManagers.RegisterLoginRequests;
using UnityEngine;
using ViewModels.Abstract;

namespace Assets.Scripts.ViewModels
{
	public class RegisterViewModel : ViewModelBase
	{
		#region Requests

		public static GameObject CreateRegisterRequestObject { get; set; }

		public void SendRegisterRequest(string name, string email, string password)
		{
			CreateRegisterRequestObject = new GameObject("ReisterRequest");

			var createRegisterRequest = CreateRegisterRequestObject.AddComponent<CreateRegisterRequest>();

			createRegisterRequest.CreateRequest(name, email, password);
		}

		#endregion

		#region Button handlers

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

		#endregion

		#region Validators

		public bool ValidateName(string name)
		{
			DataValidator validator = new DataValidator();

			bool validationResult = validator.ValidateString(name, out string message);
			if (!string.IsNullOrEmpty(message)) ShowError(message);
			return validationResult;
		}

		public bool ValidateEmail(string email)
		{
			DataValidator validator = new DataValidator();

			bool validationResult = validator.ValidateEmail(email, out string message);
			if (!string.IsNullOrEmpty(message)) ShowError(message);
			return validationResult;
		}

		public bool ValidatePassword(string password, string confirmPassword)
		{
			DataValidator validator = new DataValidator();

			bool validationResult = validator.ValidatePassword(password, out string message, confirmPassword);
			if (!string.IsNullOrEmpty(message)) ShowError(message);
			return validationResult;
		}

		private void ShowError(string message)
		{
			InformationPanelController.Instance.CreateMessage(
					InformationPanelController.MessageType.ERROR, message);
		}

		#endregion
	}
}