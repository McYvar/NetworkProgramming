using UnityEngine;

public class DeathRunCheckpoint : MonoBehaviour
{
    [SerializeField] private DeathRunGameLoop deathRunGameLoop;
    [SerializeField] public Vector3 spawnPoint;
    [SerializeField] public int checkpointId;

    private void Start()
    {
        spawnPoint += transform.position;
        deathRunGameLoop.checkpoints.Add(checkpointId, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            SessionVariables.instance.myGameClient.SendToServer(new Net_ReachedCheckpoint(SessionVariables.instance.myPlayerId, checkpointId));
            return;
        }
    }
}