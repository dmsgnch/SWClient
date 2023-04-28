using Components.Abstract;
using Newtonsoft.Json;
using Scripts.RegisterLoginScripts;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Responses.Abstract;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Components;

namespace LocalManagers.RegisterLoginScripts.Requests
{
    public class CreateLoginRequest : NetworkingManager
    {
        [SerializeField] private InputField emailField;
        [SerializeField] private InputField nameField;
        [SerializeField] private InputField passwordField;

        private const string ConnectionEndpoint = "Authentication/Login";
        
        [SerializeField] private GameObject parentCanvasObject;
        
        public void OnLoginButtonClick()
        {
            var data = new LoginRequest()
            {
                Email = emailField.text,
                Password = passwordField.text,
            };

            RestRequestForm<AuthenticationResponse> requestForm = 
                new RestRequestForm<AuthenticationResponse>(ConnectionEndpoint, 
                    RequestType.POST, parentCanvasObject, new LoginResponseHandler(),
                    JsonConvert.SerializeObject(data));

            var result = StartCoroutine( Routine_SendDataToServer<AuthenticationResponse>(requestForm));
        }
        
        public class LoginResponseHandler : IResponseHandler
        {
            public GameObject infoPanelCanvas { get; set; }
    
            public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
                where T : ResponseBase
            {
                AccessToken = requestForm.Token;
                SceneManager.LoadScene(1);
            }
        }
    }
}