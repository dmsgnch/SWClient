using System.Collections;
using UnityEngine;

public class GetterSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance is null) _instance = FindAnyObjectByType<T>();
            return _instance;
        }
    }
}

public class GetterPersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance is null) 
            {
                _instance = FindAnyObjectByType<T>();
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }
}