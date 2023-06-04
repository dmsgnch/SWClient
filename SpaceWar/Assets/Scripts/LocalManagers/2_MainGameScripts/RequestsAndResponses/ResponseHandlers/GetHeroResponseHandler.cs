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
using UnityEngine;

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
			GameManager.Instance.HeroDataStore.Color = ColorParser.GetColor((ColorStatus)hero.ColorStatus);

			HUDView hudView = UnityEngine.Object.FindAnyObjectByType<HUDView>() ?? throw new Exception();

			hudView.SetHeroNewValues(hero);

			GameManager.Instance.HeroDataStore.HeroMapView = response.Map;
			GameManager.Instance.HeroDataStore.CapitalPlanetId = hero.HomePlanetId;

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
