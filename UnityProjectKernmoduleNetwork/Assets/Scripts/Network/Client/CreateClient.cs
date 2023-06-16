using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateClient : MonoBehaviour
{
    [SerializeField] private GameObject baseClientPrefab;

    public void CreateClientObject(string ip, ushort port)
    {
        GameClient client = Instantiate(baseClientPrefab, Vector3.zero, Quaternion.identity).GetComponent<GameClient>();
        client.ip = ip;
        client.port = port;
        SessionVariables.instance.myGameClient = client;
    }
}
