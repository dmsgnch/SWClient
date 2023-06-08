using Assets.Scripts.Components;
using Assets.Scripts.Managers;
using Assets.Scripts.View;
using Components;
using Components.Abstract;
using LocalManagers.RegisterLoginRequests;
using UnityEngine;
using UnityEngine.UI;
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

		#region Commands

		public void CloseApplication(RegisterView registerView, GameObject confirmPrefab)
		{
			var panel = Object.Instantiate(confirmPrefab, registerView.gameObject.transform);

			panel.SetActive(true);
			panel.transform.GetChild(0).GetComponent<Text>().text = "Are you sure you want to quit?";
			panel.transform.GetChild(1).gameObject.
				GetComponent<Button>().onClick.AddListener(() => Application.Quit());
			panel.transform.GetChild(1).gameObject.
				GetComponent<Button>().onClick.AddListener(() => registerView.PlayButtonClickSound());

			panel.transform.GetChild(2).gameObject.
				GetComponent<Button>().onClick.AddListener(() => Object.Destroy(panel));
			panel.transform.GetChild(2).gameObject.
				GetComponent<Button>().onClick.AddListener(() => registerView.PlayButtonClickSound());
		}

		#endregion
	}
}