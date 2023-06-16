using System.Collections;
using UnityEngine;

public class SendSpawnPlayer : MonoBehaviour
{
    [SerializeField] Vector3 spawnLocation;

    private void Start()
    {
        // for each existing player, spawn one on client startup, so here...
        StartCoroutine(SpawnWhenConnected());
    }

    IEnumerator SpawnWhenConnected()
    {
        yield return new WaitUntil(() => SessionVariables.instance.connected);


        foreach (var player in SessionVariables.instance.playerDictionary)
        {
            Net_SpawnPlayer spawnPlayer = new Net_SpawnPlayer(player.Key, player.Value.playerName,
                spawnLocation.x, spawnLocation.y, spawnLocation.z);
            SessionVariables.instance.myGameClient.SendToServer(spawnPlayer);
        }
    }
}
