using UnityEngine;

public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                string path = "_" + typeof(T).FullName;
                _instance = Resources.Load<T>(path);
            }

            return _instance;
        }
    }
}
