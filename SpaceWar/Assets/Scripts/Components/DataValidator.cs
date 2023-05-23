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
		public string ValidateEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))			
				return "Displaying information panel in the future. You must fill the all of filds";
			
			string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

			if (!Regex.IsMatch(email, pattern))										
				return "Displaying information panel in the future. Email is not match to the pattern";			

			return "";
		}
	}
}
