using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Components;
using SharedLibrary.Responses.Abstract;
using UnityEngine;
using UnityEngine.Networking;

namespace Components.Abstract
{
    public interface IResponseHandler
    {
        /// <summary>
        /// Execute methods according to request result
        /// </summary>
        /// <param name="request">To get operation result</param>
        /// <param name="requestForm">Have info about this request/response</param>
        /// <typeparam name="T">Returned response type</typeparam>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ProcessResponse<T>(UnityWebRequest request, RestRequestForm<T> requestForm)
            where T : ResponseBase
        {
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    ConnectionSuccessAction(requestForm);

                    break;

                case UnityWebRequest.Result.ConnectionError:
                    ConnectionErrorAction();
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    ProtocolErrorAction(requestForm);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            OnRequestFinished();
        }

        public void ConnectionSuccessAction<T>(RestRequestForm<T> requestForm) 
            where T : ResponseBase
        {
            BodyConnectionSuccessAction<T>(requestForm);
            PostConnectionSuccessAction<T>(requestForm);
        }

        public void BodyConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
            where T : ResponseBase
        {
            CreateInfoPanels(InformationPanelController.MessageType.INFO, requestForm.Result!.Info);

			//TODO: Delay for success result information panel displaying
		}

        public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
            where T : ResponseBase
        {
            //No any post success connection action;
        }

        public void ConnectionErrorAction() 
        {
            CreateInfoPanels(InformationPanelController.MessageType.ERROR, new string[]
            {
                "Failed to establish a connection to the server. Please try again later.",
            });
        }

        public void ProtocolErrorAction<T>(RestRequestForm<T> requestForm) 
            where T : ResponseBase
        {
            CreateInfoPanels(InformationPanelController.MessageType.ERROR,
                requestForm.Result?.Info is not null
                    ? requestForm.Result.Info
                    : new string[]
                    {
                        "Server Interaction Problems"
                    });
        }

        public void OnRequestFinished()
        {
			//No any post request action;
		}

		private void CreateInfoPanels(InformationPanelController.MessageType msgType, string[] operationInfo)
        {
            if (operationInfo is null) throw new InvalidDataException("Information strings must be not null");

            foreach (string message in operationInfo)
            {
                InformationPanelController.Instance.CreateMessage(msgType, message);
            }
        }
    }
}