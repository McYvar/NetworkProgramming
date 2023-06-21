using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerPrefabNonControlable;

    private void Start()
    {
        SessionVariables.instance.myGameClient.playerSpawner = this;
    }

    public void SpawnRemotePlayer(int playerId, string playerName, Vector3 spawnLocation)
    {
        if (playerId == SessionVariables.instance.myPlayerId)
        {
            SpawnLocalPlayer(playerId, playerName, spawnLocation);
            return;
        }
        GameObject newPlayerObject = Instantiate(playerPrefabNonControlable, spawnLocation, Quaternion.identity);
        newPlayerObject.name = $"{playerName} (ID: {playerId})";
        if (!SessionVariables.instance.playerDictionary.ContainsKey(playerId))
        {
            Player newPlayer = new Player(playerId, playerName);
            newPlayer.playerObject = newPlayerObject.transform.GetChild(0).gameObject;
            newPlayer.playerObject.GetComponentInChildren<NameTag>().SetText(playerName);
            SessionVariables.instance.playerDictionary.Add(playerId, newPlayer);
        }
        else SessionVariables.instance.playerDictionary[playerId].playerObject = newPlayerObject.transform.GetChild(0).gameObject;
    }

    public void SpawnLocalPlayer(int playerId, string playerName, Vector3 spawnLocation)
    {
        GameObject newPlayerObject = Instantiate(playerPrefab, spawnLocation, Quaternion.identity);
        SessionVariables.instance.myGameClient.SendToServer(new Net_ChatMessage($"{playerName} has joined the server!"));
        newPlayerObject.name = $"{playerName} (ID: {playerId})";
        SessionVariables.instance.playerDictionary[playerId].playerObject = newPlayerObject.transform.GetChild(0).gameObject;
    }

    public void DespawnPlayer(int playerId)
    {
        if (!SessionVariables.instance.playerDictionary.ContainsKey(playerId)) return;
        if (SessionVariables.instance.playerDictionary[playerId].playerObject == null) return;
        Destroy(SessionVariables.instance.playerDictionary[playerId].playerObject);
        SessionVariables.instance.playerDictionary.Remove(playerId);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
