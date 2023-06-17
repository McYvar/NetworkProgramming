using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendPlayerTransform : MonoBehaviour
{
    [SerializeField] private float sendInterval;
    private float lastSend;

    private void Update()
    {
        if (Time.time - lastSend > sendInterval)
        {
            lastSend = Time.time;
            Net_PlayerTransform pt = new Net_PlayerTransform(SessionVariables.instance.myPlayerId, transform.position.x, transform.position.y, transform.position.z);
            SessionVariables.instance.myGameClient.SendToServer(pt);
        }
    }
}