using Assets.Scripts.Managers;
using Microsoft.AspNetCore.SignalR.Client;
using Scripts.LocalManagers;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Contracts.Hubs;
using SharedLibrary.Models;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Components;
using System.Collections.Generic;

namespace LocalManagers.ConnectToGame
{
    /// <summary>
    /// class that controls the display of players list in lobby
    /// </summary>
    public class PlayersListController : ComponentSingleton<PlayersListController>
    {
        [SerializeField] private GameObject playerPrefab;

        public void UpdatePlayersList(Lobby lobby)
        {
            foreach(Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }			
            
            foreach (var lobbyInfo in lobby.LobbyInfos)
			{
				//Create new player list item based on prefab
				var lobbyView = Instantiate(playerPrefab.transform.GetChild(0).gameObject,
					gameObject.transform);

				//Set text as username
				lobbyView.transform.GetChild(0).GetComponent<Text>().text = lobbyInfo.User.Username;

				//Disactivating toggle if lobbyLeader, otherwise make readonly 
				GameObject toggle = lobbyView.transform.GetChild(2).gameObject;
				if (lobbyInfo.LobbyLeader)
				{					
					toggle.SetActive(false);
					lobbyInfo.Ready = true;
				}
				else
				{
					toggle.GetComponent<Toggle>().interactable = false;
					toggle.GetComponent<Toggle>().isOn = lobbyInfo.Ready;
				}

				//Saving userId as component
				lobbyView.AddComponent<UserIdStorage>().UserId = lobbyInfo.UserId;

				GameObject colorButton = lobbyView.transform.GetChild(1).gameObject;
				
				colorButton.GetComponent<Image>().color = ColorParser.GetColor((ColorStatus)lobbyInfo.ColorStatus);
				if (lobbyInfo.UserId.Equals(GameManager.Instance.MainDataStore.UserId))
				{
					colorButton.AddComponent<ColorChanger>();
				}
			}
        }

		public void ChangeColor(LobbyInfo lobbyInfo)
        {
			GameObject lobbyInfoView = GetLobbyInfoView(lobbyInfo.UserId);

			GameObject colorButton = lobbyInfoView.transform.GetChild(1).gameObject;

			colorButton.GetComponent<Image>().color = ColorParser.GetColor((ColorStatus)lobbyInfo.ColorStatus);
		}

		public void ChangeReadyStatus(LobbyInfo lobbyInfo)
		{
			GameObject lobbyInfoView = GetLobbyInfoView(lobbyInfo.UserId);

			GameObject toggle = lobbyInfoView.transform.GetChild(2).gameObject;
			toggle.GetComponent<Toggle>().isOn = lobbyInfo.Ready;
		}

		public bool GetReadyStatus(Guid userId)
        {
			GameObject lobbyInfoView = GetLobbyInfoView(userId);
			Toggle toggle = lobbyInfoView.transform.GetChild(2).gameObject.GetComponent<Toggle>();
			return toggle.isOn;
		}

		private GameObject GetLobbyInfoView(Guid userId)
		{
			GameObject lobbyInfoView = null;

			foreach (Transform child in gameObject.transform)
			{
				if (child.GetComponent<UserIdStorage>().UserId.Equals(userId))
				{
					lobbyInfoView = child.gameObject;
				}
			}
			if (lobbyInfoView is null) throw new ArgumentException();
			return lobbyInfoView;
		}
	}
}