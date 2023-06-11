using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ResultRequest : MonoBehaviour
{
    private Queue<string> disconnectRequest = new Queue<string>();

    private Results result;

    private bool requesting;

    private void Start()
    {
        StartCoroutine(DisconnectOld());
        StartCoroutine(GetResultRequest(disconnectRequest.Dequeue()));
    }

    private void OnApplicationQuit()
    {
        // TO DO: APPLICATION ACTUALLY HAS TO EXECUTE THESE ON EXIT
        InstantRequest("https://studenthome.hku.nl/~yvar.toorop/php/user_logout");
        InstantRequest("https://studenthome.hku.nl/~yvar.toorop/php/server_logout");
    }

    private IEnumerator DisconnectOld()
    {
        disconnectRequest.Enqueue("https://studenthome.hku.nl/~yvar.toorop/php/user_logout");
        disconnectRequest.Enqueue("https://studenthome.hku.nl/~yvar.toorop/php/server_logout");
        yield return new WaitUntil(() => !requesting);
        if (disconnectRequest.Count > 0)
        {
            StartCoroutine(GetResultRequest(disconnectRequest.Dequeue()));
        }
    }

    public IEnumerator GetResultRequest(string uri)
    {
        requesting = true;
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
                    Debug.Log(webRequest.downloadHandler.text);
                    try
                    {
                        result = JsonUtility.FromJson<Results>(webRequest.downloadHandler.text);
                        Debug.Log(result.ToString());
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e);
                    }
                    break;
            }
        }
        requesting = false;
    }

    private void InstantRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.SendWebRequest();
        }
    }

}

[System.Serializable]
public class Results
{
    public List<Result> result;

    public override string ToString()
    {
        string message = "";
        foreach (var item in result)
        {
            message += item.ToString() + "\n";
        }
        return message;
    }
}

[System.Serializable]
public class Result
{
    public int code;
    public string session_id;
    public int server_id;
    public int user_id;
    public string email;
    public string username;

    public override string ToString()
    {
        return $"code: {code}, session_id: {session_id}, server_id: {server_id}, user_id: {user_id}, email: {email}, username: {username}";
    }
}