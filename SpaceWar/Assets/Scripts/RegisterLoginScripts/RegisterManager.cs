using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Scripts.RegisterLoginScripts;
using UnityEngine;
using UnityEngine.UI;
using SharedLibrary.Requests;
using SharedLibrary.Routes;
using Unity.VisualScripting;
using UnityEngine.Networking;

public class RegisterManager : NetworkingManager
{
    [SerializeField]
    private InputField emailField;
    [SerializeField]
    private InputField nameField;
    [SerializeField]
    private InputField passwordField;

    private string JsonData;

    private void OnRegisterButtonClick()
    {
        var data = new RegistrationRequest()
        {
            Email = emailField.text, 
            Password = passwordField.text, 
            Username = nameField.text,
        };
        
        JsonData = JsonConvert.SerializeObject(data);
        StartCoroutine(nameof(Routine_SendRegisterDataToServer));
    }

    private IEnumerator Routine_SendRegisterDataToServer()
    {
        string URIForSendDataTo = $"{BaseURL}Authentication/Register";

        using UnityWebRequest request = new UnityWebRequest(URIForSendDataTo, "POST");
        request.SetRequestHeader("Content-Type", "application/json");

        byte[] rowData = Encoding.UTF8.GetBytes(JsonData);
        request.uploadHandler = new UploadHandlerRaw(rowData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log("User has been successfully created");
                break;
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("Problems connecting to the server");
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("Server Interaction Problems");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
