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

		public void StartWithSampleData(GameObject startButton, GameObject readyButton,
			GameObject playerList, GameObject playerListItemPrefab)
        {
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
							Id = Guid.NewGuid(),
						},
						LobbyLeader = true,
						UserId = Guid.NewGuid(),
					},
					new LobbyInfo
					{
						User = new ApplicationUser
						{
							Username = "not a host",
							Id = Guid.NewGuid(),
						},
						LobbyLeader = false,
						UserId = Guid.NewGuid(),
					},
				},
			};
			GameManager.Instance.LobbyDataStore.LobbyInfo = lobby.LobbyInfos.Last();
			GameManager.Instance.LobbyDataStore.LobbyInfo.Lobby = lobby;
			DefineButton(startButton, readyButton);
			UpdatePlayersList(playerList, playerListItemPrefab);
		}

		public void UpdatePlayersList(GameObject playerList, GameObject playerListItemPrefab)
		{
			foreach (Transform child in playerList.transform)
			{
				Destroy(child.gameObject);
			}

			Lobby lobby = GameManager.Instance.LobbyDataStore.LobbyInfo.Lobby;
			Guid? hostId = GameManager.Instance.LobbyDataStore.
				LobbyInfo.Lobby.LobbyInfos.FirstOrDefault(l => l.LobbyLeader).UserId;
			if (hostId is null) return;

			foreach (var lobbyInfo in lobby.LobbyInfos)
			{
				var lobbyView = Instantiate(playerListItemPrefab.transform.GetChild(0).gameObject,
					playerList.transform);

				lobbyView.transform.GetChild(0).GetComponent<Text>().text = lobbyInfo.User.Username;
				lobbyView.transform.GetChild(1).GetComponent<Image>().color = Color.blue;

				var toggle = lobbyView.transform.GetChild(2);
				if (lobbyInfo.UserId == hostId)
				{
					toggle.gameObject.SetActive(false);
				}
				else
				{
					toggle.GetComponent<Toggle>().interactable = false;
				}
			}
		}

		public void DefineButton(GameObject startButton, GameObject readyButton)
		{
			var currentUserId = GameManager.Instance.LobbyDataStore.LobbyInfo.UserId;
			var hostId = GameManager.Instance.LobbyDataStore.LobbyInfo.Lobby.
				LobbyInfos.First(l => l.LobbyLeader).UserId;
			if (currentUserId == hostId)
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
