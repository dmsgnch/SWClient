using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameControllers;
using Newtonsoft.Json;
using SharedLibrary.Responses;
using SharedLibrary.Responses.Abstract;
using UnityEngine;
using UnityEngine.Networking;
using static GameControllers.InformationPanelController;

namespace Scripts.RegisterLoginScripts
{
    public class NetworkingManager : MonoBehaviour
    {
        public static NetworkingManager instance;

        private const string BaseURL = @"https://localhost:7148/";

        private GameObject _infoPanelParent;
        
        protected delegate void SuccessResultAction();

        private SuccessResultAction _action;

        void Start()
        {
            instance = this;
        }

        protected IEnumerator Routine_SendDataToServer<T>(string subStringForConnection, 
            string jsonData, GameObject infoPanelParent, SuccessResultAction action = null) 
            where T : IResponse
        {
            _infoPanelParent = infoPanelParent;
            _action = action;
            
            using UnityWebRequest request = new UnityWebRequest($"{BaseURL}{subStringForConnection}", "POST");
            request.SetRequestHeader("Content-Type", "application/json");

            byte[] rowData = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(rowData);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            T response = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);

            ProcessResponse<T>(request, response, action);
        }

        /*
         
         1) Перевірити тип результату:
            Якщо результат УСПІХ - Необхідно вивести повідомлення від сервера про успіх реєстрації чи успіх входу в аккаунт
                    
            Якщо результат ПОМИЛКА ПРИЄДНАННЯ ДО СЕРВЕРА - Необхідно вивести своє повідомлення, що описує цю помилку
            
            Якщо результат ПОМИЛКА ПРОТОКОЛУ - Необхідно вивести повідомлення від сервера про помилку реєстрації чи успіх входу в аккаунт
         
         */

        private void ProcessResponse<T>(UnityWebRequest request, T response, SuccessResultAction action) where T : IResponse
        {
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    CreateInfoPanels(MessageType.INFO, response.Info);
                    
                    Thread.Sleep(3 * 10^3);
                    _action?.Invoke();
                    break;

                case UnityWebRequest.Result.ConnectionError:
                    CreateInfoPanels(MessageType.ERROR, new string[]
                    {
                        "Failed to establish a connection to the server. Please try again later.",
                    });
                    
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    CreateInfoPanels(MessageType.ERROR,
                        response.Info is not null
                            ? response.Info
                            : new string[]
                            {
                                "Server Interaction Problems"
                            });

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CreateInfoPanels(MessageType msgType, string[] operationInfo)
        {
            if (operationInfo is null) throw new InvalidDataException("Information strings must be not null");

            foreach (string message in operationInfo)
            {
                _infoPanelParent.GetComponent<InformationPanelController>().CreateMessage(msgType, message);
            }
        }
    }
}