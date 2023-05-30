using Assets.Scripts.Managers;
using Assets.Scripts.ViewModels;
using Components;
using Components.Abstract;
using SharedLibrary.Responses.Abstract;
using SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Models;
using System.Data;

namespace Assets.Scripts.LocalManagers._2_MainGameScripts.RequestsAndResponses.ResponseHandlers
{
	public class GetSessionResponseHandler : IResponseHandler
	{
		public void BodyConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
			where T : ResponseBase
		{
			//Not create information panel
		}

		public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
			where T : ResponseBase
		{
			var session = requestForm.GetResponseResult<GetSessionResponse>().Session;

			GameManager.Instance.SessionDataStore.SessionId = session.Id;

			//TODO: Move to method in viewModel
			#region ToMethod

			var userId = GameManager.Instance.MainDataStore.UserId;

			foreach (Hero hero in session.Heroes)
			{
				if (hero.UserId.Equals(userId))
				{
					GameManager.Instance.HeroDataStore.HeroId = hero.HeroId;
					break;
				}
			}

			if (GameManager.Instance.HeroDataStore.HeroId.Equals(Guid.Empty))
				throw new DataException();

			#endregion

			//TODO: Execute next GetHeroRequest
		}

		public void OnRequestFinished()
		{
			//UnityEngine.Object.Destroy(ConnectToGameViewModel.GetLobbiesListRequestObject);
		}
	}
}
