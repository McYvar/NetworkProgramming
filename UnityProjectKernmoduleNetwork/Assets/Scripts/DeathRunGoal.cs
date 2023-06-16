using UnityEngine;

public class DeathRunGoal : MonoBehaviour
{
    [SerializeField] private DeathRunGameLoop deathRunGameLoop;

    private void OnTriggerEnter(Collider other)
    {
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            deathRunGameLoop.ReachedGoal(SessionVariables.instance.myPlayerId);
        }
    }
}