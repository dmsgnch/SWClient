using System.Collections;
using UnityEngine;

public class BehaviorSingleton<T> : MonoBehaviour where T : MonoBehaviour
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

public class BehaviorPersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
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

public class RegularSingleton<T> where T: new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }
}