using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Quaternion = System.Numerics.Quaternion;
using System.Linq;
using System.Threading;

namespace Components
{
    public class InformationPanelController : ComponentSingleton<InformationPanelController>
    {
        [SerializeField] private Sprite infoIcon;
        [SerializeField] private Sprite errorIcon;
        [SerializeField] private Sprite warningIcon;

        [SerializeField] private GameObject infoPanelPrefab;

		public List<GameObject> InfoPanels { get; set; } = new List<GameObject>();

		private GameObject _parentCanvas = null;

		public void CreateMessage(MessageType msgType, string message)
        {
            var parentCanvas = GetParentCanvas(gameObject);

            if (_parentCanvas is not null && !_parentCanvas.Equals(parentCanvas))
            {
				InfoPanels = new List<GameObject>();
            }

            _parentCanvas = parentCanvas;

			var infoPanel = Instantiate(infoPanelPrefab, parentCanvas.GetComponent<RectTransform>());
            
            Image objImage = infoPanel.transform.Find("IP_img").GetComponent<Image>();
            objImage.sprite = (int)msgType == 0 ? infoIcon : ((int)msgType == 1 ? errorIcon : warningIcon);
            objImage.preserveAspect = false;

            Text text = infoPanel.transform.Find("IP_tb_text").GetComponent<Text>();
            text.text = message;
            Debug.Log($"Information panel outputting: {message}");

            infoPanel.name = "InfoPanel";

			AddPanel(infoPanel);
        }

        private GameObject GetParentCanvas(GameObject canvases)
        {
            for (int i = 0; i < canvases.transform.childCount; i++)
            {
                GameObject canvas = canvases.transform.GetChild(i).gameObject;

				if (canvas.activeInHierarchy && !canvas.name.Equals("cnvs_FPS"))
                    return canvases.transform.GetChild(i).gameObject;
            }

            throw new ApplicationException("Active canvas was not found on the scene");
        }

        public void DeleteIpUtem(GameObject infoPanelObject)
        {
            if (!InfoPanels.Contains(infoPanelObject))
            {
                Debug.Log("No such info panel was found");
                return;
            }

            int infoPanelIndex = InfoPanels.IndexOf(infoPanelObject);

            InfoPanels.RemoveAt(infoPanelIndex);

            if (infoPanelIndex > 0)
            {
                for (int i = 0; i < infoPanelIndex; i++)
                {
					InfoPanels[i].transform.localPosition += new Vector3(0, 100, 0);
				}
            }
		}

        private void AddPanel(GameObject infoPanel)
        {
			InfoPanels.Add(infoPanel);

			if (InfoPanels.Count == 5)
            {
				Destroy(InfoPanels[0]);
				InfoPanels.RemoveAt(0);
            }
            foreach(GameObject i in InfoPanels)
            {
               i.transform.localPosition -= new Vector3(0, 100, 0);
            }
        }


        public enum MessageType
        {
            INFO = 0,
            ERROR = 1,
            WARNING = 2,
        }
    }
}