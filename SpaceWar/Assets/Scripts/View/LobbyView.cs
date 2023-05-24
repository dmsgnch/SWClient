using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using ViewModels.Abstract;
using View.Abstract;
using Assets.View.Abstract;
using Assets.Scripts.ViewModels;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using SharedLibrary.Models;
using Assets.Scripts.Managers;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

namespace Assets.Scripts.View
{
	public class LobbyView : AbstractScreen<LobbyViewModel>
	{
		[SerializeField] private GameObject playerList;
		[SerializeField] private GameObject playerListItemPrefab;
		[SerializeField] private GameObject startButton;
		[SerializeField] private GameObject readyButton;

		private LobbyViewModel _lobbyViewModel;

		private void OnEnable()
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
			GameManager.Instance.LobbyDataStore.LobbyInfo = lobby.LobbyInfos.First();
			GameManager.Instance.LobbyDataStore.LobbyInfo.Lobby = lobby;
			DefineButton();
			UpdatePlayersList();
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				//TODO: Add confirm window			 

				if (Debug.isDebugBuild)
				{
					Debug.Log("Application quiting");
				}
				else
				{
					Application.Quit();
				}
			}
		}

		private void UpdatePlayersList()
		{
			foreach (Transform child in playerList.transform)
			{
				Destroy(child.gameObject);
			}

			Lobby lobby = GameManager.Instance.LobbyDataStore.LobbyInfo.Lobby;
			Guid? hostId = GameManager.Instance.LobbyDataStore.
				LobbyInfo.Lobby.LobbyInfos.FirstOrDefault(l => l.LobbyLeader).UserId;
			if(hostId is null) return;

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

		private void DefineButton()
		{
			var currentUserId = GameManager.Instance.LobbyDataStore.LobbyInfo.UserId;
			var hostId = GameManager.Instance.LobbyDataStore.LobbyInfo.Lobby.
				LobbyInfos.First(l => l.LobbyLeader).UserId;
			if (currentUserId == hostId)
			{
				startButton.SetActive(true);
				readyButton.SetActive(false);
				startButton.GetComponent<Button>().onClick.AddListener(_lobbyViewModel.OnStartButtonClick);
			}
			else
			{
				startButton.SetActive(false);
				readyButton.SetActive(true);
				readyButton.GetComponent<Button>().onClick.AddListener(_lobbyViewModel.OnReadyButtonClick);
			}
		}

		protected override void OnBind(LobbyViewModel lobbyViewModel)
		{
			_lobbyViewModel = lobbyViewModel;
		}
	}
}