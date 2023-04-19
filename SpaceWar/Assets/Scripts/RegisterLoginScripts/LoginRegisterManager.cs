using Newtonsoft.Json;
using Scripts.RegisterLoginScripts;
using UnityEngine;
using UnityEngine.UI;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using UnityEngine.SceneManagement;


public class LoginRegisterManager : NetworkingManager
{
    [SerializeField] private InputField emailField;
    [SerializeField] private InputField nameField;
    [SerializeField] private InputField passwordField;

    private const string ConnectionStrRegister = "Authentication/Register";
    private const string ConnectionStrLogin = "Authentication/Login";

    [SerializeField] private GameObject parentObject;
    public void OnLoginButtonClick()
    {
        var data = new LoginRequest()
        {
            Email = emailField.text,
            Password = passwordField.text,
        };

        var result = StartCoroutine(
            Routine_SendDataToServer<AuthenticationResponse>(ConnectionStrLogin, 
                JsonConvert.SerializeObject(data), parentObject, LoginSuccessAction));
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
            Routine_SendDataToServer<AuthenticationResponse>(ConnectionStrRegister, 
                JsonConvert.SerializeObject(data), parentObject));
    }

    private void LoginSuccessAction()
    {
        SceneManager.LoadScene(1);
    }
}