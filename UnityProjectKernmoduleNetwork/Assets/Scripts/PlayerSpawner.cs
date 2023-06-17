using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerPrefabNonControlable;

    private void Start()
    {
        SessionVariables.instance.myGameClient.playerSpawner = this;
    }

    public void SpawnPlayer(int playerId, string playerName, Vector3 spawnLocation)
    {
        GameObject newPlayerObject;
        if (playerId == SessionVariables.instance.myPlayerId) newPlayerObject = Instantiate(playerPrefab, spawnLocation, Quaternion.identity);
        else newPlayerObject = Instantiate(playerPrefabNonControlable, spawnLocation, Quaternion.identity);
        newPlayerObject.name = $"{playerName} (ID: {playerId})";
        if (!SessionVariables.instance.playerDictionary.ContainsKey(playerId))
        {
            Player newPlayer = new Player(playerId, playerName);
            newPlayer.playerObject = newPlayerObject.transform.GetChild(0).gameObject;
            SessionVariables.instance.playerDictionary.Add(playerId, newPlayer);
        }
        else SessionVariables.instance.playerDictionary[playerId].playerObject = newPlayerObject.transform.GetChild(0).gameObject;

        SessionVariables.instance.myGameClient.SendToServer(new Net_ChatMessage($"{playerName} has joined the server!"));
    }
}
