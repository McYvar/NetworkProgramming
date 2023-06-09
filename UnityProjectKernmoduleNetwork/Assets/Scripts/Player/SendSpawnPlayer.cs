using System.Collections;
using UnityEngine;

public class SendSpawnPlayer : MonoBehaviour
{
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private bool debugMode;
    [SerializeField] private int debugId;
    [SerializeField] private string debugName;
    [SerializeField] private int debugId2;
    [SerializeField] private string debugName2;

    private void Start()
    {
        // for each existing player, spawn one on client startup, so here...
        StartCoroutine(SpawnWhenConnected());
    }

    IEnumerator SpawnWhenConnected()
    {

        if (debugMode)
        {
            SessionVariables.instance.playerDictionary.Add(debugId, new Player(debugId, debugName));
            SessionVariables.instance.playerDictionary.Add(debugId2, new Player(debugId2, debugName2));
        }

        yield return new WaitUntil(() => SessionVariables.instance.connected);

        // spawn all other players
        foreach (var player in SessionVariables.instance.playerDictionary.Values)
        {
            if (player.playerId == SessionVariables.instance.myPlayerId) continue;
            playerSpawner.SpawnRemotePlayer(player.playerId, player.playerName, player.position);
        }

        // then spawn self with a request
        int myPlayerId = SessionVariables.instance.myPlayerId;
        Net_SpawnPlayer spawnPlayer = new Net_SpawnPlayer(myPlayerId, SessionVariables.instance.playerDictionary[myPlayerId].playerName,
            transform.position.x, transform.position.y, transform.position.z);
        SessionVariables.instance.myGameClient.SendToServer(spawnPlayer);
    }
}
