using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.RegisterLoginScripts
{
    public class NetworkingManager : MonoBehaviour
    {
        public static NetworkingManager instance;
    
        public const string BaseURL = @"https://localhost:7148/";
        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
    }
}


