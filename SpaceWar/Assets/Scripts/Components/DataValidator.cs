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
		public bool ValidateEmail(string email, out string result)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				result = "Displaying information panel in the future. You must fill the all of filds";
				return false;
			}

			string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

			if (!Regex.IsMatch(email, pattern))
			{
				result = "Displaying information panel in the future. Email is not match to the pattern";
				return false;
			}

			result = "";
			return true;
		}

		public bool ValidateString(string input, out string result)
		{
			if (string.IsNullOrWhiteSpace(input) || input.Length > 30 || input.Length < 4)
			{
				result = "Displaying information panel in the future. The *string* must have between 4 and 30 characters";
				return false;
			}

			string pattern = @"^\w+$";

			if (!Regex.IsMatch(input, pattern))
			{
				result = "Displaying information panel in the future. The string have unavailable symbols";
				return false;
			}

			result = "";
			return true;
		}
	}
}
