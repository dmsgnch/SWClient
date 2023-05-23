using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using ViewModels.Abstract;
using View.Abstract;
using Assets.View.Abstract;
using Assets.Scripts.ViewModels;
using Unity.VisualScripting;
using UnityEngine.UIElements;

namespace Assets.Scripts.View
{
	public class ConnectToGameView : AbstractScreen<ConnectToGameViewModel>
	{
		[SerializeField] private InputField heroInput;
		[SerializeField] private InputField gameNameInput;
		[SerializeField] private UnityEngine.UI.Button quitButton;
		[SerializeField] private UnityEngine.UI.Button updateButton;
		[SerializeField] private UnityEngine.UI.Button connectToGameButton;
		[SerializeField] private UnityEngine.UI.Button CreateGameButton;
		[SerializeField] private GameObject lobbiesListItemPrefab;

		private ConnectToGameViewModel _connectToGameViewModel;

		private void Awake()
		{
			quitButton.onClick.AddListener(_connectToGameViewModel.CloseApplication);
			updateButton.onClick.AddListener(_connectToGameViewModel.OnUpdateButtonClick);
			connectToGameButton.onClick.AddListener(_connectToGameViewModel.OnToRegisterButtonClick);
			CreateGameButton.onClick.AddListener(_connectToGameViewModel.OnToRegisterButtonClick);
		}

		private void OnEnable()
		{
			_connectToGameViewModel.OnUpdateButtonClick();
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				//TODO: Add confirm window			 

				if (Debug.isDebugBuild)
				{
					Debug.Log("Application quiting");
				}
				else
				{
					Application.Quit();
				}
			}
		}

		protected override void OnBind(ConnectToGameViewModel connectToGameViewModel)
		{
			_connectToGameViewModel = connectToGameViewModel;
		}
	}
}