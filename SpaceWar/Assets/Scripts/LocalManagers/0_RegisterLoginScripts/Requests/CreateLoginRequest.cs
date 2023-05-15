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
using System.Threading;
using System.Threading.Tasks;

namespace LocalManagers.RegisterLoginScripts.Requests
{
    public class CreateLoginRequest : NetworkingManager
    {
        [SerializeField] private InputField emailField;
        [SerializeField] private InputField passwordField;

        private const string ConnectionEndpoint = "Authentication/Login";
        
        void Start()
        {
            Button btn = gameObject.GetComponent<Button>();
            btn.onClick.AddListener(OnLoginButtonClick);
        }
        
        public void OnLoginButtonClick()
        {
			var data = new LoginRequest()
			{
				Email = emailField.text,
				Password = passwordField.text,
			};

			RestRequestForm<AuthenticationResponse> requestForm =
				new RestRequestForm<AuthenticationResponse>(ConnectionEndpoint,
					RequestType.POST, new LoginResponseHandler(),
					jsonData: JsonConvert.SerializeObject(data));

			//NetworkingManager.Instance.Sending = true;

			var result = StartCoroutine(Routine_SendDataToServer<AuthenticationResponse>(requestForm));
		}
        
        public class LoginResponseHandler : IResponseHandler
        {
            public void BodyConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
				where T : ResponseBase
			{
                //Not create information panel
            }

			public void PostConnectionSuccessAction<T>(RestRequestForm<T> requestForm)
                where T : ResponseBase
            {
                NetworkingManager.Instance.AccessToken = 
                    requestForm.GetResponseResult<AuthenticationResponse>().Token;

                SceneManager.LoadScene(1);

				//NetworkingManager.Instance.Sending = false;
			}
        }
    }
}