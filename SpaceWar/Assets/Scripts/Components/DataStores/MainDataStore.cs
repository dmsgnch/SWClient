using Assets.Scripts.Components.Abstract;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Components
{
	/// <summary>
	/// Class for saving data throughout the application, including transitions between scenes
	/// </summary>
	public class MainDataStore : IDataStore
	{
		#region Access Token

		/// <summary>
		/// Token we receive after authorization and use when sending requests requiring it
		/// </summary>
		public string AccessToken { get; set; }

		#endregion


		//TODO: Find out what is that
		//It had to be a property for the name of lobby, that you input in "connect to game" input field,
		//but now we have LobbyName, so now it is redundant
		//public string LobbyToCreateName { get; set; }
	}
}
