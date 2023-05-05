using Components;
using LocalManagers.ConnectToGame;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Models;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.SignalR
{
    public class SignalRHandler : GetterSingleton<SignalRHandler>
    {
        public void Error(string errorMessage)
        {
            if (Debug.isDebugBuild) Debug.Log($"an error ocured. error message: {errorMessage}");
            InformationPanelController.Instance.CreateMessage(InformationPanelController.MessageType.ERROR,
                errorMessage);
        }

        public void DeleteLobby(string serverMessage)
        {
            InformationPanelController.Instance.CreateMessage(InformationPanelController.MessageType.INFO,
                serverMessage);
        }

        public void ConnectToLobby(Lobby lobby)
        {
            PlayersListController.Instance.UpdatePlayersList(lobby);
        }

        public void ExitFromLobby(Lobby lobby)
        {
            PlayersListController.Instance.UpdatePlayersList(lobby);
        }

        public void ChangeReadyStatus(Lobby lobby)
        {
            PlayersListController.Instance.UpdatePlayersList(lobby);
        }

        public void ChangeLobbyData(Lobby lobby)
        {

        }

        public void ChangedColor(Lobby lobby)
        {

        }

        public void CreateSession(Hero hero)
        {

        }
    }
}