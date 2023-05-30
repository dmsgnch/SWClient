using Assets.Scripts.Managers;
using Components;
using Microsoft.AspNetCore.SignalR.Client;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Contracts.Hubs;
using SharedLibrary.Models;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LocalManagers.ConnectToGame
{
	class ColorChanger : MonoBehaviour, IPointerClickHandler
	{
		public async void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
				await NextColor();
			else if (eventData.button == PointerEventData.InputButton.Right)
				await PreviousColor();
		}

		private async Task NextColor()
		{
			ColorStatus currentColorStatus = 
				ColorParser.GetColorStatus(gameObject.GetComponent<Button>().image.color);

			ColorStatus nextColorStatus = ColorParser.GetNextColor(currentColorStatus);

			HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
			Guid lobbyId = GameManager.Instance.LobbyDataStore.LobbyId;

			await hubConnection.InvokeAsync(ServerHandlers.Lobby.ChangeColor, lobbyId, (int)nextColorStatus);
		}

		private async Task PreviousColor()
		{
			ColorStatus currentColorStatus = 
				ColorParser.GetColorStatus(gameObject.GetComponent<Button>().image.color);

			ColorStatus previousColorStatus = ColorParser.GetPreviousColor(currentColorStatus);			

			HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
			Guid lobbyId = GameManager.Instance.LobbyDataStore.LobbyId;

			await hubConnection.InvokeAsync(ServerHandlers.Lobby.ChangeColor, lobbyId, (int)previousColorStatus);
		}
	}
}