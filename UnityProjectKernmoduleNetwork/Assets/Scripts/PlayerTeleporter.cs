using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    public void TeleportplayerTo(int playerId, Vector3 location)
    {
        SessionVariables.instance.playerDictionary[playerId].position = location;
    }
}