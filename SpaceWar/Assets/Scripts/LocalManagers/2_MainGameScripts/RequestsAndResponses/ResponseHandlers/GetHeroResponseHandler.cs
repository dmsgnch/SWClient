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
using Assets.Scripts.View;

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
			var session = requestForm.GetResponseResult<GetHeroResponse>().Hero;


			GameManager.Instance.HeroDataStore.HeroId = session.HeroId;
			GameManager.Instance.HeroDataStore.Name = session.Name;
			GameManager.Instance.HeroDataStore.Resourses = session.Resourses;
			GameManager.Instance.HeroDataStore.ResearchShipLimit = session.ResearchShipLimit;
			GameManager.Instance.HeroDataStore.AvailableResearchShips = session.AvailableResearchShips;
			GameManager.Instance.HeroDataStore.ColonizationShipLimit = session.ColonizationShipLimit;
			GameManager.Instance.HeroDataStore.AvailableColonizationShips = session.AvailableColonizationShips;
			GameManager.Instance.HeroDataStore.Color = ColorParser.GetColor((ColorStatus)session.ColorStatus);
						

			HUDView view = UnityEngine.Object.FindAnyObjectByType<HUDView>();

			if (view is null) throw new InvalidOperationException();

			view.UpdateHUDValues();
		}

		public void OnRequestFinished()
		{
			UnityEngine.Object.Destroy(HUDViewModel.CreateGetHeroRequestObject);
		}
	}
}
