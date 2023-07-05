using System;
using System.Collections;
using UnityEngine;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public T Get<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }

    public void GetAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
    {
        StartCoroutine(GetCoroutine(path, callback));
    }

    private IEnumerator GetCoroutine<T>(string path, Action<T> callback) where T : UnityEngine.Object
    {
        var resouce = Resources.LoadAsync<T>(path);
        yield return resouce;

        callback(resouce.asset as T);
    }
}
