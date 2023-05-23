using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SharedLibrary.Models;
using Button = UnityEngine.UI.Button;
using Scripts.RegisterLoginScripts;
using System.Linq;
using LocalManagers.ConnectToGame.Requests;
using UnityEngine.Serialization;
using Newtonsoft.Json.Linq;
using Assets.Scripts.Components;
using Assets.Scripts.Managers;

namespace LocalManagers.ConnectToGame
{
	/// <summary>
	/// class that controls the display of lobbies list
	/// </summary>
	public class LobbiesListController : ComponentSingleton<LobbiesListController>
	{
		public bool IsSelected { get; set; } = false;

		/// <summary>
		/// Testing method that displays sample data on the panel
		/// </summary>
		internal void DisplaySampleLobbies(
			GameObject lobbiesListItemPrefab,
			Button connectToGameButton)
		{
			var lobbies = Enumerable.Range(1, 25).Select(l =>
			new Lobby { Id = Guid.NewGuid(), LobbyName = $"Lobby {l}" })
				.ToList();
			UpdateLobbiesListDisplay(lobbies, lobbiesListItemPrefab, connectToGameButton);
		}

		/// <summary>
		/// Displays lobbies on lobby panel using lobby prefab
		/// </summary>
		/// <param name="lobbies">Collection of lobbies that must be displayed</param>
		internal void UpdateLobbiesListDisplay(
			IList<Lobby> lobbies,
			GameObject lobbiesListItemPrefab,
			Button connectToGameButton)
		{
			foreach (Transform child in gameObject.transform)
			{
				Destroy(child.gameObject);
			}

			if (lobbies is null) return;

			foreach (var lobby in lobbies)
			{
				var lobbyView = Instantiate(lobbiesListItemPrefab.transform.GetChild(0).gameObject,
					gameObject.transform);
				lobbyView.transform.GetChild(0).GetComponent<Text>().text = lobby.LobbyName;

				var lobbyButton = lobbyView.GetComponent<Button>();
				lobbyButton.onClick.RemoveAllListeners();
				lobbyButton.onClick.AddListener(() =>
				{
					GameManager.Instance.MainDataStore.SelectedLobbyId = lobby.Id.ToString();

					IsSelected = true;

//					if (!string.IsNullOrWhiteSpace(GameManager.Instance.MainDataStore.HeroName) &&
//InputValidator.Validate(GameManager.Instance.MainDataStore.HeroName))
//					{
//						connectToGameButton.interactable = true;
//					}
//					else
//					{
//						connectToGameButton.interactable = false;
//					}
				});
			}
		}
	}
}