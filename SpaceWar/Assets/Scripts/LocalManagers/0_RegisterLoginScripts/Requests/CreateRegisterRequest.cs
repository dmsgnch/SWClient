using Components.Abstract;
using Newtonsoft.Json;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using UnityEngine;
using UnityEngine.UI;
using Components;

namespace LocalManagers.RegisterLoginScripts.Requests
{
    public class CreateRegisterRequest : NetworkingManager
    {
        [SerializeField] private InputField emailField;
        [SerializeField] private InputField nameField;
        [SerializeField] private InputField passwordField;

        private const string ConnectionEndpoint = "Authentication/Register";

        void Start()
        {
            Button btn = gameObject.GetComponent<Button>();
            btn.onClick.AddListener(OnRegisterButtonClick);
        }
        
        public void OnRegisterButtonClick()
        {
            var data = new RegistrationRequest()
            {
                Email = emailField.text,
                Password = passwordField.text,
                Username = nameField.text,
            };
        
            RestRequestForm<AuthenticationResponse> requestForm = 
                new RestRequestForm<AuthenticationResponse>(ConnectionEndpoint, 
                    RequestType.POST, new RegisterResponseHandler(),
                    jsonData: JsonConvert.SerializeObject(data));

            var result = StartCoroutine( Routine_SendDataToServer<AuthenticationResponse>(requestForm));
        }
    }
    
    public class RegisterResponseHandler : IResponseHandler
    { }
}