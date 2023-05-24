using Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assets.Scripts.Components
{
	public class DataValidator
	{
		public bool ValidateEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				InformationPanelController.Instance.CreateMessage(
					InformationPanelController.MessageType.WARNING,
					"Email cannot be empty!");
				return false;
			}

			string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

			if (!Regex.IsMatch(email, pattern))
			{
				InformationPanelController.Instance.CreateMessage(
					InformationPanelController.MessageType.WARNING,
					"Invalid email!");
				return false;
			}

			return true;
		}

		public bool ValidateString(string input)
		{
			if (string.IsNullOrWhiteSpace(input) || input.Length > 30 || input.Length < 4)
			{
				//InformationPanelController.Instance.CreateMessage(
				//	InformationPanelController.MessageType.WARNING,
				//	"Value length must have between 4 and 30 characters!");
				return false;
			}

			string pattern = @"^\w+$";

			if (!Regex.IsMatch(input, pattern))
			{
				//InformationPanelController.Instance.CreateMessage(
				//	InformationPanelController.MessageType.WARNING,
				//	"Value is invalid!");
				return false;
			}

			return true;
		}

		public bool ValidatePassword(string password, string confirmPassword = null)
        {
			// Check password length
			if (password.Length < 8)
			{
				InformationPanelController.Instance.CreateMessage(
					InformationPanelController.MessageType.WARNING,
					"Password must be at least 8 characters long!");
				return false;
			}

			// Check for at least one uppercase letter
			if (!password.Any(c => char.IsUpper(c)))
			{
				InformationPanelController.Instance.CreateMessage(
					InformationPanelController.MessageType.WARNING, 
					"Password must contain at least one uppercase letter!");
				return false;
			}

			// Check for at least one lowercase letter
			if (!password.Any(c => char.IsLower(c)))
			{
				InformationPanelController.Instance.CreateMessage(
					InformationPanelController.MessageType.WARNING,
					"Password must contain at least one lowercase letter!");
				return false;
			}

			// Check for at least one digit
			if (!password.Any(c => char.IsDigit(c)))
			{
				InformationPanelController.Instance.CreateMessage(
					InformationPanelController.MessageType.WARNING,
					"Password must contain at least one digit!");
				return false;
			}

			// Check for equality between password and confirm password  
			if (confirmPassword is not null && password != confirmPassword)
			{
				InformationPanelController.Instance.CreateMessage(
					InformationPanelController.MessageType.WARNING,
					"Password must contain at least one digit!");
				return false;
			}

			// All checks passed, password is valid
			return true;
		}
	}
}
