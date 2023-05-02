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

namespace Components
{
    public class InformationPanelController : MonoBehaviour
    {
        [SerializeField] private Sprite infoIcon;
        [SerializeField] private Sprite errorIcon;
        [SerializeField] private Sprite warningIcon;

        [SerializeField] private GameObject infoPanelPrefab;
        [SerializeField] private List<GameObject> infoPanels;
        public void CreateMessage(MessageType msgType, string message)
        {
            var infoPanel = Instantiate(infoPanelPrefab,GetComponent<RectTransform>());
            infoPanels.Add(infoPanel);
            Image objImage = infoPanel.transform.Find("IP_img").GetComponent<Image>();
            objImage.sprite = (int)msgType == 0 ? infoIcon : ((int)msgType == 1 ? errorIcon : warningIcon);
            objImage.preserveAspect = false;

            Text text = infoPanel.transform.Find("IP_tb_text").GetComponent<Text>();
            text.text = message;
            ControlPanels();
        }
        public void ControlPanels()
        {
            if (infoPanels.Count > 5)
            {
                Destroy(infoPanels[0]);
                infoPanels.RemoveAt(0);
            }
            foreach(GameObject i in infoPanels)
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