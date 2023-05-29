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
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
				NextColor();
			else if (eventData.button == PointerEventData.InputButton.Right)
				PreviousColor();
		}

		private async Task NextColor()
		{
			var image = gameObject.GetComponent<Button>().image;

			var colorParser = new ColorParser();
			Color currentColor = image.color;
			ColorStatus currentColorStatus = colorParser.GetColorStatus(currentColor);
			ColorStatus nextColorStatus;
			//scary magic for getting last element of enum
			ColorStatus lastColor = (ColorStatus)Enum.GetValues(typeof(ColorStatus)).Cast<int>().Max();
			if (currentColorStatus.Equals(lastColor))
			{
				//scary magic for getting first element of enum
				ColorStatus firstColor = (ColorStatus)Enum.GetValues(typeof(ColorStatus)).Cast<int>().Min();
				nextColorStatus = firstColor;
			}
            else
            {
				nextColorStatus = currentColorStatus+1;
            }

			HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
			Guid lobbyId = GameManager.Instance.LobbyDataStore.LobbyId;
			await hubConnection.InvokeAsync(ServerHandlers.Lobby.ChangeColor, lobbyId, (int)nextColorStatus);
		}

		private async Task PreviousColor()
		{
			var image = gameObject.GetComponent<Button>().image;

			var colorParser = new ColorParser();
			Color currentColor = image.color;
			ColorStatus currentColorStatus = colorParser.GetColorStatus(currentColor);
			ColorStatus previousColorStatus;
			//scary magic for getting first element of enum
			ColorStatus firstColor = (ColorStatus)Enum.GetValues(typeof(ColorStatus)).Cast<int>().Min();
			if (currentColorStatus.Equals(firstColor))
			{
				//scary magic for getting last element of enum
				ColorStatus lastColor = (ColorStatus)Enum.GetValues(typeof(ColorStatus)).Cast<int>().Max();
				previousColorStatus = lastColor;
			}
			else
			{
				previousColorStatus = currentColorStatus-1;
			}

			HubConnection hubConnection = NetworkingManager.Instance.HubConnection;
			Guid lobbyId = GameManager.Instance.LobbyDataStore.LobbyId;
			await hubConnection.InvokeAsync(ServerHandlers.Lobby.ChangeColor, lobbyId, (int)previousColorStatus);
		}
	}
}