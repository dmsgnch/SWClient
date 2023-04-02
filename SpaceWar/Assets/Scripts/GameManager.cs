using System.Net.Http;
using SharedLibrary;
using UnityEngine;
using Scripts;
using HttpClient = Scripts.HttpClient;


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        var player = await HttpClient.Get<Player>("https://localhost:44355/player/500");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
