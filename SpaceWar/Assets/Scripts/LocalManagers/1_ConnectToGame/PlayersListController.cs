using Assets.Scripts.Managers;
using Microsoft.AspNetCore.SignalR.Client;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Contracts.Hubs;
using SharedLibrary.Models;
using System;
using UnityEngine;
using UnityEngine.UI;

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
				//create new player list item based on prefab
				var lobbyView = Instantiate(playerPrefab.transform.GetChild(0).gameObject,
					gameObject.transform);

				lobbyView.transform.GetChild(0).GetComponent<Text>().text = lobbyInfo.User.Username;
				GameObject toggle = lobbyView.transform.GetChild(2).gameObject;
				if (lobbyInfo.LobbyLeader)
				{
					toggle.SetActive(false);
				}
				else
				{
					toggle.GetComponent<Toggle>().interactable = false;
				}

				GameObject colorButton = lobbyView.transform.GetChild(1).gameObject;

				//if (lobbyInfo.Color is not null)
				//{
				//	float r = lobbyInfo.Color.R;
				//	float g = lobbyInfo.Color.G;
				//	float b = lobbyInfo.Color.B;
				//	float a = lobbyInfo.Color.A;
				//	colorButton.GetComponent<Image>().color = new Color(r,g,b,a);
				//}
				//            else
				//            {
				//	colorButton.GetComponent<Image>().color = Color.blue;
				//	Color color = colorButton.GetComponent<Image>().color;
				//	byte[] argbBytes = { 
				//		(byte)color.a,
				//		(byte)color.r,
				//		(byte)color.g,
				//		(byte)color.b,
				//	};
				//	int argb = BitConverter.ToInt32(argbBytes, 0);
				//	HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
				//	hubConnection.InvokeAsync(ServerHandlers.Lobby.ChangeColor, lobby.Id, argb).Wait();
				//}
				colorButton.GetComponent<Image>().color = Color.blue;
				if (lobbyInfo.UserId == GameManager.Instance.MainDataStore.UserId)
				{
					colorButton.AddComponent<ColorChanger>().SetButton(colorButton.GetComponent<Button>());
				}
			}
        }
    }
}