using Components.Abstract;
using Newtonsoft.Json;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using UnityEngine;
using UnityEngine.UI;
using Components;
using static Assets.Scripts.ViewModels.RegisterViewModel;
using Assets.Scripts.LocalManagers._0_RegisterLoginRequests.ResponseHandlers;

namespace LocalManagers.RegisterLoginRequests
{
    public class CreateRegisterRequest : MonoBehaviour
    {
        [SerializeField] private InputField emailField;
        [SerializeField] private InputField nameField;
        [SerializeField] private InputField passwordField;

        private const string ConnectionEndpoint = "Authentication/Register";

        public void CreateRequest(string name, string email, string password)
        {
            var data = new RegistrationRequest()
            {
                Username = name,
                Email = email,
                Password = password,                
            };
        
            RestRequestForm<AuthenticationResponse> requestForm = 
                new RestRequestForm<AuthenticationResponse>(ConnectionEndpoint, 
                    RequestType.POST, new RegisterResponseHandler(),
                    jsonData: JsonConvert.SerializeObject(data));

            var result = StartCoroutine(NetworkingManager.Instance.Routine_SendDataToServer<AuthenticationResponse>(requestForm));
        }
    }   
}