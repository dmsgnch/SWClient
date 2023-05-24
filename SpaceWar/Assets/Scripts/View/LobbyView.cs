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
			_lobbyViewModel.StartWithSampleData(startButton, readyButton,playerList,playerListItemPrefab);
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

		protected override void OnBind(LobbyViewModel lobbyViewModel)
		{
			_lobbyViewModel = lobbyViewModel;
		}
	}
}