using Assets.Scripts.Components.Abstract;
using Assets.Scripts.Managers;
using LocalManagers.RegisterLoginRequests;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using ViewModels.Abstract;

namespace Assets.Scripts.ViewModels
{
	public class LobbyViewModel : ViewModelBase
	{
		CreateLoginRequest createLoginRequest;

		public LobbyViewModel()
		{ }

		public void OnStartButtonClick()
		{

		}

		public void OnReadyButtonClick()
		{

		}

		/// <summary>
		/// generates sample data for testing lobby functionality
		/// </summary>
		/// <returns>lobby filled with sample data</returns>
		public Lobby GetSampleData()
		{
			var userId1 = Guid.NewGuid();
			var userId2 = Guid.NewGuid();
			var lobby = new Lobby
			{
				LobbyName = "default lobby",
				LobbyInfos = new LobbyInfo[]
				{
					new LobbyInfo
					{
						User = new ApplicationUser
						{
							Username = "host",
							Id = userId1,
						},
						LobbyLeader = true,
						UserId = userId1,
					},
					new LobbyInfo
					{
						User = new ApplicationUser
						{
							Username = "not a host",
							Id = userId2,
						},
						LobbyLeader = false,
						UserId = userId2,
					},
				},
			};
			GameManager.Instance.LobbyDataStore.IsLobbyLeader = lobby.LobbyInfos.Last().LobbyLeader;
			return lobby;
		}

		/// <summary>
		/// updates list of players with new state of lobby
		/// </summary>
		/// <param name="playerList">
		/// GameObject for list of players that is being updated
		/// </param>
		/// <param name="playerListItemPrefab">
		/// GameObject for a prefab for item in players list
		/// </param>
		/// <param name="lobby">
		/// new state of lobby
		/// </param>
		public void UpdatePlayersList(GameObject playerList, GameObject playerListItemPrefab, Lobby lobby)
		{
			//Clear players list
			foreach (Transform child in playerList.transform)
			{
				Destroy(child.gameObject);
			}

			foreach (var lobbyInfo in lobby.LobbyInfos)
			{
				var lobbyView = Instantiate(playerListItemPrefab.transform.GetChild(0).gameObject,
					playerList.transform);

				lobbyView.transform.GetChild(0).GetComponent<Text>().text = lobbyInfo.User.Username;
				lobbyView.transform.GetChild(1).GetComponent<Image>().color = Color.blue;

				var toggle = lobbyView.transform.GetChild(2);
				if (lobbyInfo.LobbyLeader)
				{
					toggle.gameObject.SetActive(false);
				}
				else
				{
					toggle.GetComponent<Toggle>().interactable = false;
				}
			}
		}

		/// <summary>
		/// defines whether lobby view should have start button or ready button 
		/// depending on whether current user is lobby leader
		/// </summary>
		/// <param name="startButton">
		/// start button, which should be viewed when current user is a lobby leader
		/// </param>
		/// <param name="readyButton">
		/// ready button, which should be viewed when current user is not a lobby leade
		/// r</param>
		public void DefineButton(GameObject startButton, GameObject readyButton)
		{
			if (GameManager.Instance.LobbyDataStore.IsLobbyLeader)
			{
				startButton.SetActive(true);
				readyButton.SetActive(false);
				startButton.GetComponent<Button>().onClick.AddListener(OnStartButtonClick);
			}
			else
			{
				startButton.SetActive(false);
				readyButton.SetActive(true);
				readyButton.GetComponent<Button>().onClick.AddListener(OnReadyButtonClick);
			}
		}

		private void OnCoroutineFinishedEventHandler()
		{
			Destroy(createLoginRequest.gameObject);
		}
	}
}
