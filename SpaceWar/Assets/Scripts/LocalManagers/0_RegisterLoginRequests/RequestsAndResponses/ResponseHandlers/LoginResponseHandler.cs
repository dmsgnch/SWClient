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

namespace Assets.Scripts.LocalManagers._0_RegisterLoginRequests.ResponseHandlers
{
	public class LoginResponseHandler : IResponseHandler
	{
		public void BodyConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
			where T : ResponseBase
		{
			//Not create information panel
		}

		public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
			where T : ResponseBase
		{
			var authResponse = requestForm.GetResponseResult<AuthenticationResponse>();
			GameManager.Instance.MainDataStore.AccessToken = authResponse.Token;
			GameManager.Instance.MainDataStore.UserId = authResponse.UserId;

			GameManager.Instance.ChangeState(GameState.LoadConnectToGameScene);
		}

		public void OnRequestFinished()
		{
			UnityEngine.Object.Destroy(LoginViewModel.CreateLoginRequestObject);
		}
	}
}
