using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        SessionVariables.instance.gameClient.playerSpawner = this;
    }

    public void SpawnPlayer(Vector3 spawnLocation)
    {
        GameObject newPlayer = Instantiate(playerPrefab, spawnLocation, Quaternion.identity);
        newPlayer.name = $"{SessionVariables.instance.playerName} (ID: {SessionVariables.instance.playerId})";
    }
}
