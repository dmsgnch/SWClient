using Assets.Scripts.Components;
using Assets.Scripts.Managers;
using Components.Abstract;
using Components;
using LocalManagers.RegisterLoginRequests;
using SharedLibrary.Responses.Abstract;
using SharedLibrary.Responses;
using UnityEngine;
using ViewModels.Abstract;

namespace Assets.Scripts.ViewModels
{
	public class MainMenuViewModel : ViewModelBase
	{
		public void CloseApplication(GameObject confirmPrefab)
		{
			var panel = MonoBehaviour.Instantiate(confirmPrefab);

			//TODO: Set prefab values

			Application.Quit();
		}
	}
}
