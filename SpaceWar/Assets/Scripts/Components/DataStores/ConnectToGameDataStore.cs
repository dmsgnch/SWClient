using Assets.Scripts.Components.Abstract;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class ConnectToGameDataStore : IDataStore
    {
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
		public Guid SelectedLobbyId { get; set; }

		/// <summary>
		/// List of the lobbies using for displaying list in ConnectToGame scene
		/// </summary>
		public IList<Lobby> Lobbies { get; set; } = new List<Lobby>(0);
	}
}