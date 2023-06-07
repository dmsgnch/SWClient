using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LocalManagers.Menu
{
    internal class MenuButtonController : MonoBehaviour
    {
        private void Start()
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => 
            {
                Button button = gameObject.GetComponent<Button>();
                Color buttonColor = button.GetComponent<Image>().color;
                button.GetComponent<Image>().color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, 1f);

                gameObject.GetComponentInChildren<Text>().color = Color.black;
            });

            EventTrigger.Entry exit = new EventTrigger.Entry();
            exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener((eventData) =>
            {
                Button button = gameObject.GetComponent<Button>();
                Color buttonColor = button.GetComponent<Image>().color;
                button.GetComponent<Image>().color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, 0f);

                gameObject.GetComponentInChildren<Text>().color = Color.white;
            });

            gameObject.GetComponent<EventTrigger>().triggers.Add(entry);
            gameObject.GetComponent<EventTrigger>().triggers.Add(exit);
        }

        //private void Update()
        //{
        //    if (EventSystem.current.IsPointerOverGameObject())
        //    {
        //        Button button = gameObject.GetComponent<Button>();
        //        Color buttonColor = button.GetComponent<Image>().color;
        //        button.GetComponent<Image>().color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, 1f);

        //        gameObject.GetComponentInChildren<Text>().color = Color.black;
        //        isRepainted = true;
        //    }
        //    else if(isRepainted)
        //    {
        //        Button button = gameObject.GetComponent<Button>();
        //        Color buttonColor = button.GetComponent<Image>().color;
        //        button.GetComponent<Image>().color = new Color(buttonColor.r, buttonColor.g, buttonColor.b, 0f);

        //        gameObject.GetComponentInChildren<Text>().color = Color.white;
        //        isRepainted = false;
        //    }
        //}
    }
}
