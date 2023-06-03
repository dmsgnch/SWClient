using Assets.Scripts.Managers;
using Assets.Scripts.ViewModels;
using Assets.View.Abstract;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.View
{
	public class MainGameCameraView : AbstractScreen<MainGameCameraViewModel>
	{
		//[SerializeField]
		//private GameObject _camera;


		private bool MouseMove { get; set; } = false;
		private Vector3 MousePosition;
		private float ScrollInput;
		private float CurrentDistance = -200;


		private MainGameCameraViewModel _mainGameCameraViewModel;

        public void Awake()
        {
        }

        private void OnEnable()
        {
        }

        private void Update()
		{
			if (_mainGameCameraViewModel is null) return;

            if (Input.GetMouseButton(0))
			{
				MouseMove = true;

				_mainGameCameraViewModel.OnMouseMove(gameObject, ref MousePosition);
			}
			else
			{
				MouseMove = false;
			}

			if (!MouseMove)
            {
                MousePosition = Input.mousePosition;
                _mainGameCameraViewModel.OnBorderMove(gameObject, ref MousePosition);
			}

			ScrollInput = Input.GetAxis("Mouse ScrollWheel");

			_mainGameCameraViewModel.OnZoom(ref ScrollInput, ref CurrentDistance, gameObject);
		}

        public void CenterCameraOnCapital()
        {
			_mainGameCameraViewModel.CenterCameraOnCapital(gameObject);
        }

        protected override void OnBind(MainGameCameraViewModel mainGameCameraViewModel)
		{
			_mainGameCameraViewModel = mainGameCameraViewModel;
        }
	}
}
