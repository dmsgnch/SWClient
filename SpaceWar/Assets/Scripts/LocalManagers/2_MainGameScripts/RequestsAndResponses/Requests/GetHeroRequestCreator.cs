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
	public class GetHeroRequestCreator : MonoBehaviour
	{
		private const string ConnectionEndpoint = "hero/";

		public void CreateRequest()
		{
			Guid heroId = GameManager.Instance.HeroDataStore.HeroId;

			if (heroId.Equals(Guid.Empty))
				throw new ArgumentException();

			RestRequestForm<GetHeroResponse> requestForm =
				new RestRequestForm<GetHeroResponse>(ConnectionEndpoint + heroId,
					RequestType.GET, new GetHeroResponseHandler(),
					token: GameManager.Instance.MainDataStore.AccessToken);

			var result = StartCoroutine(NetworkingManager.Instance.Routine_SendDataToServer(requestForm));
		}
	}
}
