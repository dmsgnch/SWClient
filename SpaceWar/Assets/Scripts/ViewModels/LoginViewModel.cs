using Assets.Scripts.Components;
using Assets.Scripts.Managers;
using Components.Abstract;
using Components;
using LocalManagers.RegisterLoginRequests;
using SharedLibrary.Responses.Abstract;
using SharedLibrary.Responses;
using UnityEngine;
using ViewModels.Abstract;
using Assets.Scripts.View;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace Assets.Scripts.ViewModels
{
	public class LoginViewModel : ViewModelBase
	{
		#region Requests

		public static GameObject CreateLoginRequestObject { get; set; }

		public void SendLoginRequest(string email, string password)
		{
			CreateLoginRequestObject = new GameObject("LoginRequest");

			var createLoginRequest = CreateLoginRequestObject.AddComponent<CreateLoginRequest>();

			createLoginRequest.CreateRequest(email, password);			
		}

		#endregion

		#region Buttons handlers

		public void ToRegister()
		{
			GameManager.Instance.ChangeState(GameState.Register);
		}

		#endregion

		#region Validators

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

		#endregion

		#region Commands

		private GameObject ConfirmationPanel { get; set; }

		public void CloseApplication(LoginView loginView, GameObject confirmPrefab)
		{
			if (!ConfirmationPanel.IsDestroyed() && ConfirmationPanel is not null) return;

			ConfirmationPanel = Object.Instantiate(confirmPrefab, loginView.gameObject.transform);

			ConfirmationPanel.SetActive(true);
			ConfirmationPanel.transform.GetChild(0).GetComponent<Text>().text = "Are you sure you want to quit?";
			ConfirmationPanel.transform.GetChild(1).gameObject.
				GetComponent<Button>().onClick.AddListener(() =>
				{
					loginView.PlayButtonClickSound();
					Application.Quit();
				});

			ConfirmationPanel.transform.GetChild(2).gameObject.
				GetComponent<Button>().onClick.AddListener(() =>
				{
					loginView.PlayButtonClickSound();
					Object.Destroy(ConfirmationPanel);
				});
		}

		#endregion
	}
}
