using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LocalManagers.ConnectToGame
{
    public static class InputValidator
    {
        public static bool Validate(string input)
        {
            if(input.Length > 20 || input.Length < 4)
            {
                return false;
            }

            var regex = new Regex(@"^\w+$");
            return regex.IsMatch(input);
        }
    }
}