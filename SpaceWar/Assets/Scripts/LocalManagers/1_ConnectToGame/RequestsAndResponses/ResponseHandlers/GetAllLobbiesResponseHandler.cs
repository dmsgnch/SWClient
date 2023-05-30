using Assets.Scripts.Managers;
using Components.Abstract;
using Components;
using LocalManagers.RegisterLoginRequests;
using SharedLibrary.Responses.Abstract;
using SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.ViewModels;
using LocalManagers.ConnectToGame.Requests;
using System.IO;
using LocalManagers.ConnectToGame;

namespace Assets.Scripts.LocalManagers._0_RegisterLoginRequests.ResponseHandlers
{
	public class GetAllLobbiesResponseHandler : IResponseHandler
	{
		public void BodyConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
			where T : ResponseBase
		{
			//Not create information panel
		}

		public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
			where T : ResponseBase
		{
			GameManager.Instance.ConnectToGameDataStore.Lobbies =
				requestForm.GetResponseResult<GetAllLobbiesResponse>().Lobbies;
		}
        public void ProtocolErrorAction<T>(RestRequestForm<T> requestForm)
            where T : ResponseBase
        {
            if (requestForm.Result?.Info is null) throw new InvalidDataException("Information strings must be not null");

            foreach (string message in requestForm.Result?.Info)
            {
                InformationPanelController.Instance.CreateMessage(InformationPanelController.MessageType.ERROR, message);
            }
			LobbiesListController.Instance.ClearList();
        }

        public void OnRequestFinished()
		{
			UnityEngine.Object.Destroy(ConnectToGameViewModel.GetLobbiesListRequestObject);
		}
	}
}
