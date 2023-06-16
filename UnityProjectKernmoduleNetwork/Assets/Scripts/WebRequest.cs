using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    public IEnumerator Request<T>(string uri, Action<T> callback)
    {
        T result = default(T);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("connection error");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log("dat processing error");

                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("protocol error");

                    break;
                case UnityWebRequest.Result.Success:
                    try
                    {
                        Debug.Log($"{uri}\n{webRequest.downloadHandler.text}");
                        result = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e);
                    }
                    break;
            }
        }
        callback?.Invoke(result);
    }
}
