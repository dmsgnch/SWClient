using System;
using System.Threading.Tasks;
using Components.Abstract;
using JetBrains.Annotations;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Responses;
using SharedLibrary.Responses.Abstract;
using UnityEngine;

namespace Components
{
    public class RestRequestForm<T> where T : ResponseBase
    {
        public string EndPoint { get; }
        public RequestType RequestType { get; }
        public IResponseHandler ResponseHandler { get; }
        
        [CanBeNull] public string Token { get; set; }
        [CanBeNull] public string JsonData { get; }
        [CanBeNull] public T Result { get; set; } = null;

        public RestRequestForm(string endPoint,
            RequestType requestType,
            IResponseHandler handler,
            [CanBeNull] string token = null,
            [CanBeNull] string jsonData = null)
        {
            EndPoint = endPoint;
            RequestType = requestType;
            ResponseHandler = handler;
            Token = token;
            JsonData = jsonData;
        }

        /// <summary>
        /// Checks that the class is a generic type
        /// </summary>
        /// <typeparam name="K">Invariant class of ResponseBase</typeparam>
        /// <returns>Query execution result in generic type</returns>
        public K GetResponseResult<K>() 
            where K : ResponseBase
        {
            if (Result is K) return Result as K;
            return null;
		}
    }
}