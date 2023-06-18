using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    private void Start()
    {
        SessionVariables.instance.myGameClient.playerTeleporter = this;
    }

    public void TeleportplayerTo(int playerId, Vector3 location)
    {
        SessionVariables.instance.playerDictionary[playerId].position = location;
        SessionVariables.instance.playerDictionary[playerId].playerObject.transform.position = location;
        SessionVariables.instance.playerDictionary[playerId].playerObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}