using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class InternetUtility
{

    public static IEnumerator CheckInternetConnection(Action<bool> action)
    {
        var request = new UnityWebRequest("https://playfab.com");
        request.timeout = 5;
        yield return request.SendWebRequest();


        switch (request.result)
        {
            case UnityWebRequest.Result.Success:
                action(true);
                break;
            default:
                Debug.Log(request.responseCode);
                action(false);
                break;
        }
    }
}
