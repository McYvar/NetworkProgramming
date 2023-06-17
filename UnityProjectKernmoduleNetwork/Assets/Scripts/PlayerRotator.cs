using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private float smoothRotationTime;

    private void Start()
    {
        SessionVariables.instance.myGameClient.playerRotator = this;
    }

    public void RotatePlayer(int playerId, float xDir, float yDir, float zDir)
    {
        if (playerId == SessionVariables.instance.myPlayerId) return;
        SessionVariables.instance.playerDictionary[playerId].gravityDirection = new Vector3(xDir, yDir, zDir);
    }

    private void Update()
    {
        foreach (var player in SessionVariables.instance.playerDictionary.Values)
        {
            if (player.playerId == SessionVariables.instance.myPlayerId) continue;
            if (player.playerObject == null) continue;
            player.playerObject.transform.rotation = Quaternion.Slerp(player.playerObject.transform.rotation, Quaternion.FromToRotation(Vector3.down, player.gravityDirection), smoothRotationTime);
        }
    }
}
