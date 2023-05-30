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
	public class RegisterResponseHandler : IResponseHandler
	{
		public void OnRequestFinished()
		{
			UnityEngine.Object.Destroy(RegisterViewModel.CreateRegisterRequestObject);
		}
	}
}
