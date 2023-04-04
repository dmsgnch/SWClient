using System.Net.Http;
using SharedLibrary;
using UnityEngine;
using Scripts;
using SharedLibrary.Requests;
using HttpClient = Scripts.HttpClient;
using SharedLibrary.Responses;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
	private AuthenticationResponse response;
	// Start is called before the first frame update
	async void Start()
	{
		//AuthenticationResponse response = new AuthenticationResponse();
		AuthenticationRequest data = new AuthenticationRequest() { Username = "Name", Password = "password69" };
		string path = "https://localhost:7148/authentication/register";

		response = await HttpClient.Post<AuthenticationResponse>(path, data);
	}

	// Update is called once per frame
	void Update()
	{
		if(response != null) Debug.Log(response.Token);
	}
}
