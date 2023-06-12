using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Servers
{
    public List<Server> servers;

    public override string ToString()
    {
        string result = "";
        if (servers != null)
        {
            foreach (var server in servers)
            {
                result += server.ToString() + "\n";
            }
        }
        return result;
    }
}

[System.Serializable]
public class Server
{
    public int code;
    public int server_id;
    public string server_name;
    public string ip;
    public int port;
    public int local;

    public Server()
    {
        code = 0;
        server_id = -1;
        server_name = null;
        ip = null;
        port = 0;
        local = -1;
    }

    public override string ToString()
    {
        return $"code: {code}, server_id: {server_id}, server_name: {server_name}, ip: {ip}, local: {local}";
    }
}
