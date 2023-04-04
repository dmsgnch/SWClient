using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Scripts
{

	public class HttpClient
	{
		public static async Task<T> Get<T>(string endpoint)
		{
			var getRequest = CreateRequest(endpoint);
			getRequest.SendWebRequest();

			while (!getRequest.isDone) await Task.Delay(10);
			return JsonConvert.DeserializeObject<T>(getRequest.downloadHandler.text);
		}

		public static async Task<T> Post<T>(string endpoint, object payload) where T: class
		{
			var postRequest = CreateRequest(endpoint, RequestType.POST, payload);
			postRequest.SendWebRequest();

			while (!postRequest.isDone) await Task.Delay(10);

			Debug.Log($"Request result: {postRequest.result}");

			// TODO: If result is not success we get string (not T object) and method below throws an error
			return JsonConvert.DeserializeObject<T>(postRequest.downloadHandler.text);
			
			
			//Debug.LogError(JsonConvert.DeserializeObject<string>(postRequest.downloadHandler.text));
			//return new Task<T>(() => new T() {JsonConvert.DeserializeObject<string>(postRequest.downloadHandler.text)});


		}

		private static UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET,
			object data = null)
		{
			var request = new UnityWebRequest(path, type.ToString());

			request.SetRequestHeader("Content-Type", "application/json");

			if (data != null)
			{
				var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
				request.uploadHandler = new UploadHandlerRaw(bodyRaw);
			}

			request.downloadHandler = new DownloadHandlerBuffer();			

			return request;
		}

		private static void AttachHeader(UnityWebRequest request, string key, string value)
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