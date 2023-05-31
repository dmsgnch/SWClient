using Assets.Scripts.LocalManagers._2_MainGameScripts.RequestsAndResponses.Requests;
using LocalManagers.RegisterLoginRequests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ViewModels.Abstract;

namespace Assets.Scripts.ViewModels
{
	public class HUDViewModel : ViewModelBase
	{
		public static GameObject CreateGetSessionRequestObject { get; set; }
		public static GameObject CreateGetHeroRequestObject { get; set; }

		public void UpdateStatusBar(ref GameObject statusBar) {
		
		}

		public void GetSessionRequestCreate()
		{
			CreateGetSessionRequestObject = new GameObject("GetSessionRequest");

			var createGetSessionRequest = CreateGetSessionRequestObject.AddComponent<GetSessionRequest>();

			createGetSessionRequest.CreateRequest();
		}

		public void GetHeroRequestCreate()
		{
			CreateGetHeroRequestObject = new GameObject("GetHeroRequest");

			var createGetHeroRequest = CreateGetHeroRequestObject.AddComponent<GetHeroRequestCreator>();

			createGetHeroRequest.CreateRequest();
		}

		public void CreateResourcePanel(GameObject ResourcesInfoPanelPrefab)
		{
			MonoBehaviour.Instantiate(ResourcesInfoPanelPrefab);
		}
	}
}
