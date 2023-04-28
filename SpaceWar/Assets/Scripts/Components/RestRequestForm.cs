using System;
using System.Threading.Tasks;
using Components.Abstract;
using JetBrains.Annotations;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Responses.Abstract;
using UnityEngine;

namespace Components
{
    public class RestRequestForm<T> where T : ResponseBase
    {
        public string EndPoint { get; }
        public RequestType RequestType { get; }
        public GameObject InformationPanelCanvas { get; set; }
        public IResponseHandler ResponseHandler { get; }
        
        [CanBeNull] public string Token { get; set; }
        [CanBeNull] public string JsonData { get; }
        [CanBeNull] public T Result { get; set; } = null;

        public RestRequestForm(string endPoint,
            RequestType requestType,
            GameObject infoPanelCanvas,
            IResponseHandler handler,
            [CanBeNull] string token = null,
            [CanBeNull] string data = null)
        {
            EndPoint = endPoint;
            RequestType = requestType;
            InformationPanelCanvas = infoPanelCanvas;
            ResponseHandler = handler;
            Token = token;
            JsonData = data;
        }
    }
}