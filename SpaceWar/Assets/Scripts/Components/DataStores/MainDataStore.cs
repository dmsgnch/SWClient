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


		#region Parameters ConnectToGame canvas

		/// <summary>
		/// The name of the lobby that we enter in the ConnectToGame scene and use to create a new lobby
		/// </summary>
		public string LobbyName { get; set; }

		/// <summary>
		/// The name of the hero that we enter in the ConnectToGame scene and use while create a new game
		/// </summary>
		public string HeroName { get; set; }

		/// <summary>
		/// Id of the lobby that we select from the list ob the ConnectToGame scene
		/// </summary>
		public string SelectedLobbyId { get; set; }

		/// <summary>
		/// List of the lobbies using for displaying list in ConnectToGame scene
		/// </summary>
		public IList<Lobby> Lobbies { get; set; } = new List<Lobby>(0);

		#endregion

		#region Parameters Lobby canvas

		/// <summary>
		/// Id of the lobby we are in
		/// </summary>
		public Guid LobbyId { get; set; }

		#endregion


		//TODO: Find out what is that
		public string LobbyToCreateName { get; set; }
	}
}
