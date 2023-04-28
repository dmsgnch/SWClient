using System;
using System.Collections;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using SharedLibrary.Responses.Abstract;
using UnityEngine.Networking;
using Components;
using SharedLibrary.Models;


namespace Scripts.RegisterLoginScripts
{
    public class NetworkingManager : StaticInstance<NetworkingManager>
    {
        private const string BaseURL = @"https://localhost:7148/";

        protected static string AccessToken { get; set; }

        public static Guid LobbyId { get; set; }

        public static string LobbyName { get; set; }

        #region SignalR
        
        // public HubConnection HubConnection { get; set; }
        //
        // private void Start()
        // {
        //     HubConnection = new HubConnectionBuilder()
        //         .WithAutomaticReconnect()
        //         .WithUrl($"{BaseURL}") //Add substr
        //         .Build();
        //
        //     HubConnection.On<string, string>("Receive", (user, message) =>
        //     {
        //         var newMessage = $"{user}: {message}";
        //     });
        // }
        
        #endregion

        #region REST
        
        protected IEnumerator Routine_SendDataToServer<T>(RestRequestForm<T> requestForm)
            where T : ResponseBase
        {
            using UnityWebRequest request = new UnityWebRequest($"{BaseURL}{requestForm.EndPoint}",
                requestForm.RequestType.ToString());
            request.SetRequestHeader("Content-Type", "application/json");

            if (requestForm.JsonData is not null)
            {
                byte[] rowData = Encoding.UTF8.GetBytes(requestForm.JsonData);
                request.uploadHandler = new UploadHandlerRaw(rowData);
            }

            request.downloadHandler = new DownloadHandlerBuffer();

            if (!String.IsNullOrWhiteSpace(requestForm.Token))
            {
                AttachHeader(request, "Authorization", "token");
            }

            yield return request.SendWebRequest();

            requestForm.Result = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);

            requestForm.ResponseHandler.ProcessResponse(request, requestForm);
        }
        
        #endregion
        
        private void AttachHeader(UnityWebRequest request, string key, string value)
        {
            request.SetRequestHeader(key, value);
        }
    }
    
    public enum RequestType
    {
        GET = 0,
        POST = 1,
        PUT = 2
    }
}