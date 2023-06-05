using Assets.Scripts.Components;
using Assets.Scripts.Managers;
using Components.Abstract;
using Components;
using LocalManagers.RegisterLoginRequests;
using SharedLibrary.Responses.Abstract;
using SharedLibrary.Responses;
using UnityEngine;
using ViewModels.Abstract;
using UnityEngine.UI;

namespace Assets.Scripts.ViewModels
{
	public class MainMenuViewModel : ViewModelBase
	{
		public void CloseApplication(GameObject confirmPrefab, GameObject parent)
		{
			var panel = MonoBehaviour.Instantiate(confirmPrefab, parent.transform);
			//panel.transform.SetParent(parent.transform);
            panel.SetActive(true);
			panel.transform.GetChild(0).GetComponent<Text>().text = "Are you sure you want to quit?";
			panel.transform.GetChild(1).gameObject.
				GetComponent<Button>().onClick.AddListener(()=> Application.Quit());
            panel.transform.GetChild(2).gameObject.
                GetComponent<Button>().onClick.AddListener(() => Object.Destroy(panel));
		}
	}
}
