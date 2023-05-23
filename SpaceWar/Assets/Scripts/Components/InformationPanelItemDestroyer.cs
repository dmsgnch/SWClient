using Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Assets.Scripts.Components
{
	public class InformationPanelItemDestroyer : MonoBehaviour
	{
		private Button _button;

		private void Start()
		{
			_button = GetComponent<Button>();
			if (_button is null) throw new DataMisalignedException();

			_button.onClick.AddListener(OnButtonClick);
		}

		private void OnButtonClick()
		{
			GameObject infoPanelGameObject = gameObject.transform.parent.gameObject;
			InformationPanelController.Instance.DeleteIpUtem(infoPanelGameObject);
			Destroy(infoPanelGameObject);
		}
	}
}
