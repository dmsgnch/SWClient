using Scripts.RegisterLoginScripts;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.LocalManagers._1_ConnectToGame
{
    public class StartButtonController : BehaviorSingleton<StartButtonController>
    {
        [SerializeField] private GameObject _startButton;
        [SerializeField] private GameObject _readyButton;

        public void DefineButton()
        {
            var currentUserId = NetworkingManager.Instance.LobbyInfo.UserId;
            var hostId = NetworkingManager.Instance.LobbyInfo.Lobby.LobbyInfos.First(l => l.LobbyLeader).UserId;
            if (currentUserId == hostId)
            {
                _startButton.SetActive(true);
                _readyButton.SetActive(false);
            }
            else
            {
                _startButton.SetActive(false);
                _readyButton.SetActive(true);
            }
        }
    }
}