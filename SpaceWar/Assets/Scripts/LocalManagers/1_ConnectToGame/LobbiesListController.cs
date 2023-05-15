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

namespace LocalManagers.ConnectToGame
{
	/// <summary>
	/// class that controls the display of lobbies list
	/// </summary>
	public class LobbiesListController : BehaviorSingleton<LobbiesListController>
	{
		[SerializeField] private GameObject lobbiesListItemPrefab;
		[SerializeField] private Button ConnectToGameButton;

		public void OnEnable()
		{
			Debug.Log($"LobbiesListController OnEnable method");
			DisplayLobbyList();
		}

		public void GetList()
		{
			GetLobbiesListRequest getLobbiesListRequest = new GetLobbiesListRequest();
			getLobbiesListRequest.GetLobbyList();
		}

		private void DisplayLobbyList()
		{
			//Debug:
			//DisplaySampleLobbies();

			if (ConnectToGameButton.IsActive())
			{
				new GetLobbiesListRequest().GetLobbyList();

				UpdateLobbiesListDisplay(NetworkingManager.Instance.Lobbies);
			}
		}

		/// <summary>
		/// testing function that displays sample data on the panel
		/// </summary>
		private void DisplaySampleLobbies()
		{
			var lobbies = Enumerable.Range(1, 25).Select(l =>
			new Lobby { Id = Guid.NewGuid(), LobbyName = $"Lobby {l}" })
				.ToList();
			UpdateLobbiesListDisplay(lobbies);
		}

		/// <summary>
		/// displays lobbies on lobby panel using lobby template
		/// </summary>
		/// <param name="lobbies">collection of lobbies that you want to be displayed</param>
		private void UpdateLobbiesListDisplay(IList<Lobby> lobbies)
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
					NetworkingManager.Instance.SelectedLobbyId = lobby.Id.ToString();

					if (!string.IsNullOrWhiteSpace(NetworkingManager.Instance.HeroName) &&
					InputValidator.Validate(NetworkingManager.Instance.HeroName))
					{
						ConnectToGameButton.interactable = true;
					}
					else
					{
						ConnectToGameButton.interactable = false;
					}
				});
			}
		}
	}
}