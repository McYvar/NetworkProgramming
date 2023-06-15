using UnityEngine;

public class SendSpawnPlayer : MonoBehaviour
{
    [SerializeField] Vector3 spawnLocation;

    private void Start()
    {
        Net_SpawnPlayer spawnPlayer = new Net_SpawnPlayer(SessionVariables.instance.playerId,
            spawnLocation.x, spawnLocation.y, spawnLocation.z);
        SessionVariables.instance.gameClient.SendToServer(spawnPlayer);
    }
}
