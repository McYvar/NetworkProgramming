using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateServer : MonoBehaviour
{
    [SerializeField] private GameObject baseServerPrefab;

    public void CreateServerObject(string ip, ushort port)
    {
        GameServer server = Instantiate(baseServerPrefab, Vector3.zero, Quaternion.identity).GetComponent<GameServer>();
        server.ip = ip;
        server.port = port;
        SessionVariables.instance.server = server;
    }
}
