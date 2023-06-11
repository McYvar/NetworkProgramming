using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerRequest : MonoBehaviour
{
    public List<Server> servers = new List<Server>();

    private void Start()
    {
        UpdateServerList();
    }

    public void UpdateServerList()
    {
        StartCoroutine(GetServersRequest("https://studenthome.hku.nl/~yvar.toorop/php/server_get_all_servers"));
    }

    private IEnumerator GetServersRequest(string uri)
    {
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
                        servers = JsonUtility.FromJson<ServerList>(webRequest.downloadHandler.text).servers;
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e);
                    }
                    break;
            }
        }
    }

    public void SetServer(int serverId)
    {
        StartCoroutine(GetServersRequest($"https://studenthome.hku.nl/~yvar.toorop/php/server_login?server_id={serverId}&password=password"));
    }
}


[System.Serializable]
public class ServerList
{
    public List<Server> servers;
}

[System.Serializable]
public class Server
{
    public int server_id;
    public string server_name;
    public string ip;
    public bool local;
}
