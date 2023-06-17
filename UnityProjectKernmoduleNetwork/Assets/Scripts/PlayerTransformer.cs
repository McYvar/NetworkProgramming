using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformer : MonoBehaviour
{
    [SerializeField] private float smoothTransformTime;

    private void Start()
    {
        SessionVariables.instance.myGameClient.playerTransformer = this;
    }

    public void TransformPlayer(int playerId, float xPos, float yPos, float zPos)
    {
        if (playerId == SessionVariables.instance.myPlayerId) return;
        SessionVariables.instance.playerDictionary[playerId].position = new Vector3(xPos, yPos, zPos);
    }

    private void Update()
    {
        foreach (var player in SessionVariables.instance.playerDictionary.Values)
        {
            if (player.playerId == SessionVariables.instance.myPlayerId) continue;
            if (player.playerObject == null) continue;
            player.playerObject.transform.position = Vector3.SmoothDamp(player.playerObject.transform.position, player.position, ref player.smoothTransformVelocity, smoothTransformTime);
        }
    }
}
