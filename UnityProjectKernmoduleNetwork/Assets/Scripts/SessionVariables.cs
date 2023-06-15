using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionVariables : MonoBehaviour
{
    public Dictionary<int, GameObject> playerDictionary = new Dictionary<int, GameObject>(); // player id, player
    public GameClient gameClient;
    public BaseServer server;

    public int serverId;
    public int playerId;
    public string playerName;


    public static SessionVariables instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        
        DontDestroyOnLoad(this);
        instance = this;
    }
}
