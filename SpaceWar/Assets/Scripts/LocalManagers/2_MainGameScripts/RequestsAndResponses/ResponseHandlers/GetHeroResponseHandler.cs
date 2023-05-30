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

namespace Assets.Scripts.LocalManagers._2_MainGameScripts.RequestsAndResponses.ResponseHandlers
{
	public class GetHeroResponseHandler : IResponseHandler
	{
		public void BodyConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
			where T : ResponseBase
		{
			//Not create information panel
		}

		public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
			where T : ResponseBase
		{
			//TODO: Display hero and heroMapView data
		}

		public void OnRequestFinished()
		{
			//UnityEngine.Object.Destroy(ConnectToGameViewModel.GetLobbiesListRequestObject);
		}
	}
}
