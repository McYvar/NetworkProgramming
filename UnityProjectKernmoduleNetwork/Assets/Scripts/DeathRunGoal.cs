using UnityEngine;

public class DeathRunGoal : MonoBehaviour
{
    [SerializeField] private DeathRunGameLoop deathRunGameLoop;

    private void OnTriggerEnter(Collider other)
    {
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            SessionVariables.instance.myGameClient.SendToServer(new Net_ReachedGoal(SessionVariables.instance.myPlayerId));
            return;
        }
    }
}