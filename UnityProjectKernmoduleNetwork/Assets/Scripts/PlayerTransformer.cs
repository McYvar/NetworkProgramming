using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformer : MonoBehaviour
{
    [SerializeField] private float smoothTransformTime;
    [SerializeField] private float smoothRotationTime;

    private void Start()
    {
        SessionVariables.instance.myGameClient.playerTransformer = this;
    }

    public void TransformPlayer(int playerId, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot)
    {
        if (playerId == SessionVariables.instance.myPlayerId) return;
        //if (!SessionVariables.instance.playerDictionary.ContainsKey(playerId)) { Debug.Log(playerId); return; }
        SessionVariables.instance.playerDictionary[playerId].position = new Vector3(xPos, yPos, zPos);
        SessionVariables.instance.playerDictionary[playerId].eulerAngles = new Vector3(xRot, yRot, zRot);
    }

    private void Update()
    {
        foreach (var player in SessionVariables.instance.playerDictionary.Values)
        {
            if (player.playerId == SessionVariables.instance.myPlayerId) continue;
            if (player.playerObject == null) continue;
            player.playerObject.transform.position = Vector3.SmoothDamp(player.playerObject.transform.position, player.position, ref player.smoothTransformVelocity, smoothTransformTime);
            player.playerObject.transform.eulerAngles = Vector3.SmoothDamp(player.playerObject.transform.eulerAngles, player.eulerAngles, ref player.smoothRotationVelocity, smoothRotationTime);
        }
    }
}
