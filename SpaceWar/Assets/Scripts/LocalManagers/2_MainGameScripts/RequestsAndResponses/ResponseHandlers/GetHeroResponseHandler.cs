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
using SharedLibrary.Models.Enums;

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
			GetHeroResponse response = requestForm.GetResponseResult<GetHeroResponse>();
            Hero hero = response.Hero;

			GameManager.Instance.HeroDataStore.HeroId = hero.HeroId;
			GameManager.Instance.HeroDataStore.Name = hero.Name;
			GameManager.Instance.HeroDataStore.Resourses = hero.Resourses;
			GameManager.Instance.HeroDataStore.SoldiersLimit = hero.SoldiersLimit;
			GameManager.Instance.HeroDataStore.AvailableSoldiers = hero.AvailableSoldiers;
			GameManager.Instance.HeroDataStore.Resourses = hero.Resourses;
			GameManager.Instance.HeroDataStore.ResearchShipLimit = hero.ResearchShipLimit;
			GameManager.Instance.HeroDataStore.AvailableResearchShips = hero.AvailableResearchShips;
			GameManager.Instance.HeroDataStore.ColonizationShipLimit = hero.ColonizationShipLimit;
			GameManager.Instance.HeroDataStore.AvailableColonizationShips = hero.AvailableColonizationShips;
			GameManager.Instance.HeroDataStore.Color = ColorParser.GetColor((ColorStatus)hero.ColorStatus);
			GameManager.Instance.HeroDataStore.HeroMapView = response.Map;
			GameManager.Instance.HeroDataStore.CapitalPlanetId = hero.HomePlanetId;

            HUDView hudView = UnityEngine.Object.FindAnyObjectByType<HUDView>();
			if (hudView is null) throw new InvalidOperationException();
			hudView.UpdateHUDValues();

			PlanetsView planetsView = UnityEngine.Object.FindAnyObjectByType<PlanetsView>();
			if (planetsView is null) throw new InvalidOperationException();
			planetsView.GeneratePlanetsWithConnections();

            MainGameCameraView mainCameraView = UnityEngine.Object.FindAnyObjectByType<MainGameCameraView>();
            if (mainCameraView is null) throw new InvalidOperationException();
            mainCameraView.CenterCameraOnCapital();
        }

		public void OnRequestFinished()
		{
			UnityEngine.Object.Destroy(HUDViewModel.CreateGetHeroRequestObject);
		}
	}
}
