using System;
using UnityEngine;

namespace Components
{
    public class ChangeActiveObjects : Singleton<ChangeActiveObjects>
    {
        /// <summary>
        /// Change active status for the objects on the scene by the name.
        /// Using for changing active canvases.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void SwapActivity(string firstObject, string secondObject)
        {
            GameObject firstGameObject = gameObject.transform.Find(firstObject).gameObject;
            GameObject secondGameObject = gameObject.transform.Find(secondObject).gameObject;

            if (firstGameObject.activeInHierarchy)
            {
                firstGameObject.SetActive(false);
                secondGameObject.SetActive(true);
            }
            else if (secondGameObject.activeInHierarchy)
            {
                firstGameObject.SetActive(true);
                secondGameObject.SetActive(false);
            }
            else
            {
                throw new ArgumentException("One of the game objects must be not active " +
                                            "and the other one must be active on the scene");
            }
            
        }
    }
}