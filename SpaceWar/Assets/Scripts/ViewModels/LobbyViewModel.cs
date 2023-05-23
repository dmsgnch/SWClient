using Assets.Scripts.Components.Abstract;
using Assets.Scripts.Managers;
using LocalManagers.RegisterLoginRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using ViewModels.Abstract;

namespace Assets.Scripts.ViewModels
{
	public class LobbyViewModel : ViewModelBase
	{		
		CreateLoginRequest createLoginRequest;

		public LobbyViewModel()
		{ }

		public void OnLoginButtonClick()
		{	
			//createLoginRequest = new GameObject().AddComponent<CreateLoginRequest>();

			//createLoginRequest.onCoroutineFinished.AddListener(OnCoroutineFinishedEventHandler);

			//createLoginRequest.CreateRequest();
		}

		private void OnCoroutineFinishedEventHandler()
		{
			Destroy(createLoginRequest.gameObject);
		}
	}
}
