using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Scripts.RegisterLoginScripts;
using UnityEngine;
using UnityEngine.UI;
using SharedLibrary.Requests;
using SharedLibrary.Routes;
using Unity.VisualScripting;
using UnityEngine.Networking;
using GameControllers;
using SharedLibrary.Responses;
using static GameControllers.InformationPanelController;

public class LoginRegisterManager : NetworkingManager
{
    [SerializeField] private InputField emailField;
    [SerializeField] private InputField nameField;
    [SerializeField] private InputField passwordField;

    [SerializeField] private InformationPanelController infoPanelManager;

    private string _jsonData;
    private AuthenticationResponse _response;

    private const string ConnectionStrRegister = "Authentication/Register";
    private const string ConnectionStrLogin = "Authentication/Login";
    
    private static string _connectionStrForSending;

    public void OnLoginButtonClick()
    {
        var data = new LoginRequest()
        {
            Email = emailField.text,
            Password = passwordField.text,
        };

        _jsonData = JsonConvert.SerializeObject(data);
        _connectionStrForSending = $"{BaseURL}{ConnectionStrLogin}";
        
        StartCoroutine(nameof(Routine_SendRegisterDataToServer));
    }

    public void OnRegisterButtonClick()
    {
        var data = new RegistrationRequest()
        {
            Email = emailField.text,
            Password = passwordField.text,
            Username = nameField.text,
        };

        _jsonData = JsonConvert.SerializeObject(data);
        _connectionStrForSending = $"{BaseURL}{ConnectionStrRegister}";
        
        StartCoroutine(nameof(Routine_SendRegisterDataToServer));
    }

    private IEnumerator Routine_SendRegisterDataToServer()
    {
        using UnityWebRequest request = new UnityWebRequest(_connectionStrForSending, "POST");
        request.SetRequestHeader("Content-Type", "application/json");

        byte[] rowData = Encoding.UTF8.GetBytes(_jsonData);
        request.uploadHandler = new UploadHandlerRaw(rowData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        _response = JsonConvert.DeserializeObject<AuthenticationResponse>(request.downloadHandler.text);
        
        ProcessResponse(request);
    }
    
    /*
     
     1) Перевірити тип результату:
        Якщо результат УСПІХ - Необхідно вивести повідомлення від сервера про успіх реєстрації чи успіх входу в аккаунт
                
        Якщо результат ПОМИЛКА ПРИЄДНАННЯ ДО СЕРВЕРА - Необхідно вивести своє повідомлення, що описує цю помилку
        
        Якщо результат ПОМИЛКА ПРОТОКОЛУ - Необхідно вивести повідомлення від сервера про помилку реєстрації чи успіх входу в аккаунт
     
     */

    private void ProcessResponse(UnityWebRequest request)
    {
        switch (request.result)
        {
            case UnityWebRequest.Result.Success:
                CreateInfoPanels(MessageType.INFO, _response.Info);
                
                Debug.Log("User has been successfully created");
                break;
            
            case UnityWebRequest.Result.ConnectionError:
                CreateInfoPanels(MessageType.ERROR, new string[]
                {
                    "Failed to establish a connection to the server. Please try again later.",
                });
                
                Debug.Log("Problems connecting to the server");
                break;
            
            case UnityWebRequest.Result.ProtocolError:
                CreateInfoPanels(MessageType.ERROR, 
                                 _response.Info is not null ? _response.Info : new string[]
                {
                    "Server Interaction Problems"
                });

                Debug.Log("Server Interaction Problems");
            
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
            infoPanelManager.CreateMessage(msgType, message);
        }
        
    }
}