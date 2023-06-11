using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Servers
{
    public List<Server> servers;

    public override string ToString()
    {
        string result = "";
        foreach (var server in servers)
        {
            result += server.ToString() + "\n";
        }
        return result;
    }
}

[System.Serializable]
public class Server
{
    public int code;
    public int id;
    public string server_name;
    public string ip;
    public int local;

    public Server()
    {
        code = 0;
        id = -1;
        server_name = null;
        ip = null;
        local = -1;
    }

    public override string ToString()
    {
        return $"server_id: {id}, server_name: {server_name}, ip: {ip}, local: {local}";
    }
}
