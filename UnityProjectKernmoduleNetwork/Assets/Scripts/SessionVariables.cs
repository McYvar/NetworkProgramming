using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionVariables : MonoBehaviour
{
    public static GameClient gameClient;

    private static int serverId;
    private static int playerId;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public static void SetServerId(int newServerId)
    {
        serverId = newServerId;
    }

    public static void SetPlayerId(int newPlayerId)
    {
        playerId = newPlayerId;
    }
}
