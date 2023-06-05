using Assets.Scripts.LocalManagers._0_RegisterLoginRequests.ResponseHandlers;
using Assets.Scripts.LocalManagers._2_MainGameScripts.RequestsAndResponses.ResponseHandlers;
using Assets.Scripts.Managers;
using Components;
using Newtonsoft.Json;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LocalManagers._2_MainGameScripts.RequestsAndResponses.Requests
{
	public class GetSessionRequest : MonoBehaviour
	{
		private const string ConnectionEndpoint = "session/";

		public void CreateRequest()
		{
			Guid sessionId = GameManager.Instance.SessionDataStore.SessionId;

			if (sessionId.Equals(Guid.Empty))
				throw new ArgumentException();

			RestRequestForm<GetSessionResponse> requestForm =
				new RestRequestForm<GetSessionResponse>(ConnectionEndpoint + sessionId,
					RequestType.GET, new GetSessionResponseHandler(),
					token: GameManager.Instance.MainDataStore.AccessToken);

			var result = StartCoroutine(NetworkingManager.Instance.Routine_SendDataToServer(requestForm));
		}
	}
}
