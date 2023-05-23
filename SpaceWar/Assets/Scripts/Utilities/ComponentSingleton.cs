using System.Collections;
using System.Data;
using UnityEngine;

public class ComponentSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = FindAnyObjectByType<T>();
            }

            if (_instance is null) throw new DataException("Component was not found");
                    
            return _instance;
        }
    }

	protected void Awake()
	{
		_instance = this as T;
	}
}

public class ComponentPersistentSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance is null) 
            {
                _instance = FindAnyObjectByType<T>();
				if (_instance is null) throw new DataException("Component was not found");

				DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

	protected void Awake()
	{
		if (_instance == null)
		{
			_instance = this as T;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
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