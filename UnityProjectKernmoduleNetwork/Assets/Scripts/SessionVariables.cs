using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionVariables : MonoBehaviour
{
    public Dictionary<int, Player> playerDictionary = new Dictionary<int, Player>(); // player id, playername
    public GameClient myGameClient;
    public GameServer server;

    public bool connected = false;

    public string sessionId;
    public int serverId;
    public int myPlayerId;
    public string myPlayerName;

    public static SessionVariables instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        instance = this;
    }
}

public class Player
{
    public int playerId;
    public string playerName;
    public GameObject playerObject;

    public Vector3 position;
    public Vector3 smoothTransformVelocity;

    public Vector3 gravityDirection;

    public Player(int playerId, string playerName)
    {
        this.playerId = playerId;
        this.playerName = playerName;
        playerObject = null;
    }
}