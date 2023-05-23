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
using Assets.Scripts.Components;
using Assets.Scripts.Managers;

namespace LocalManagers.RegisterLoginRequests
{
    public class CreateLoginRequest : NetworkingManager
    {
        private const string ConnectionEndpoint = "Authentication/Login";

        public void CreateRequest(string email, string password)
        {
			var data = new LoginRequest()
			{
				Email = email,
				Password = password,
			};

			RestRequestForm<AuthenticationResponse> requestForm =
				new RestRequestForm<AuthenticationResponse>(ConnectionEndpoint,
					RequestType.POST, new LoginResponseHandler(),
					jsonData: JsonConvert.SerializeObject(data));

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
                GameManager.Instance.MainDataStore.AccessToken = 
                    requestForm.GetResponseResult<AuthenticationResponse>().Token;

                GameManager.Instance.ChangeState(GameState.LoadConnectToGameScene);
			}
        }
    }
}