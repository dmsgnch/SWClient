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
    public class InformationPanelController : BehaviorSingleton<InformationPanelController>
    {
        [SerializeField] private Sprite infoIcon;
        [SerializeField] private Sprite errorIcon;
        [SerializeField] private Sprite warningIcon;

        [SerializeField] private GameObject infoPanelPrefab;
        private IList<GameObject> _infoPanels = new List<GameObject>();
        
        public void CreateMessage(MessageType msgType, string message)
        {
            var parentCanvas = GetParentCanvas(gameObject);
            
            var infoPanel = Instantiate(infoPanelPrefab, parentCanvas.GetComponent<RectTransform>());
            _infoPanels.Add(infoPanel);
            
            Image objImage = infoPanel.transform.Find("IP_img").GetComponent<Image>();
            objImage.sprite = (int)msgType == 0 ? infoIcon : ((int)msgType == 1 ? errorIcon : warningIcon);
            objImage.preserveAspect = false;

            Text text = infoPanel.transform.Find("IP_tb_text").GetComponent<Text>();
            text.text = message;
            ControlPanels();
        }

        private GameObject GetParentCanvas(GameObject canvases)
        {
            for (int i = 0; i < canvases.transform.childCount; i++)
            {
                if (canvases.transform.GetChild(i).gameObject.activeInHierarchy)
                    return canvases.transform.GetChild(i).gameObject;
            }

            throw new ApplicationException("Active canvas was not found on the scene");
        }

        public void ControlPanels()
        {
            if (_infoPanels.Count > 5)
            {
                Destroy(_infoPanels[0]);
                _infoPanels.RemoveAt(0);
            }
            foreach(GameObject i in _infoPanels)
            {
               i.transform.localPosition -= new Vector3(0,50,0);
            }
          //  Debug.Log(infoPanels.Count);
        }
        public enum MessageType
        {
            INFO = 0,
            ERROR = 1,
            WARNING = 2,
        }
    }
}