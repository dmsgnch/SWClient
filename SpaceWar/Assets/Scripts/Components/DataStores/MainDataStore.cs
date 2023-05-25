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

		/// <summary>
		/// id of authorizated user
		/// </summary>
		public Guid UserId { get; set; }
	}
}
