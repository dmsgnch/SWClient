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


public class LoginRegisterManager : NetworkingManager
{
    [SerializeField] private InputField emailField;
    [SerializeField] private InputField nameField;
    [SerializeField] private InputField passwordField;

    private const string ConnectionStrRegister = "Authentication/Register";
    private const string ConnectionStrLogin = "Authentication/Login";

    public void OnLoginButtonClick()
    {
        var data = new LoginRequest()
        {
            Email = emailField.text,
            Password = passwordField.text,
        };

        StartCoroutine(
            Routine_SendDataToServer<AuthenticationResponse>(ConnectionStrLogin, JsonConvert.SerializeObject(data)));
    }

    public void OnRegisterButtonClick()
    {
        var data = new RegistrationRequest()
        {
            Email = emailField.text,
            Password = passwordField.text,
            Username = nameField.text,
        };

        StartCoroutine(
            Routine_SendDataToServer<AuthenticationResponse>(ConnectionStrRegister, JsonConvert.SerializeObject(data)));
    }
}