using Assets.Scripts.Components.Abstract;
using Assets.Scripts.View;
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

		private IList<Lobby> lobbies= new List<Lobby>();

		/// <summary>
		/// List of the lobbies using for displaying list in ConnectToGame scene
		/// </summary>
		public IList<Lobby> Lobbies 
		{ 
			get => lobbies;
			set
			{
				lobbies = value;
				ConnectToGameView connectToGameView = MonoBehaviour.FindAnyObjectByType<ConnectToGameView>();
				if (connectToGameView is null) throw new Exception();
				connectToGameView.OnLobbiesListUpdate();
			}
		}
	}
}