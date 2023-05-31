using Assets.Scripts.ViewModels;
using Assets.View.Abstract;
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
		[SerializeField]
		private GameObject _camera;

		private bool MouseMoove { get; set; } = false;
		private Vector3 MousePosition;
		private float ScrollInput;
		private float CurrentDistance;


		private MainGameCameraViewModel _mainGameCameraViewModel;

		void Start()
		{
			CurrentDistance = _camera.transform.position.z;
		}

		public void Awake()
		{
			//_camera
		}

		private void Update()
		{
			if (_mainGameCameraViewModel is null) return;

			MousePosition = Input.mousePosition;

			if (Input.GetMouseButton(0))
			{
				MouseMoove = true;

				_mainGameCameraViewModel.OnMouseMove(_camera, ref MousePosition);
			}
			else
			{
				MouseMoove = false;
			}

			if (!MouseMoove)
			{
				_mainGameCameraViewModel.OnBorderMove(_camera, ref MousePosition);
			}

			ScrollInput = Input.GetAxis("Mouse ScrollWheel");

			_mainGameCameraViewModel.OnZoom(ref ScrollInput, ref CurrentDistance, _camera);
		}

		private async void OnEnable()
		{
			if (_mainGameCameraViewModel is null) return;
		}

		protected override void OnBind(MainGameCameraViewModel mainGameCameraViewModel)
		{
			_mainGameCameraViewModel = mainGameCameraViewModel;
		}
	}
}
