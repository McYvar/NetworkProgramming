using UnityEngine;

public class DeathRunCheckpoint : MonoBehaviour
{
    [SerializeField] private DeathRunGameLoop deathRunGameLoop;
    private int checkpointId;

    private void Start()
    {
        checkpointId = deathRunGameLoop.checkpoints.Count;
        deathRunGameLoop.checkpoints.Add(this);
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