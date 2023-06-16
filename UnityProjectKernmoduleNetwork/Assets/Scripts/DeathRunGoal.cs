using UnityEngine;

public class DeathRunGoal : MonoBehaviour
{
    [SerializeField] private DeathRunGameLoop deathRunGameLoop;

    private void OnTriggerEnter(Collider other)
    {
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            foreach (var player in SessionVariables.instance.playerDictionary.Values)
            {
                if (player.playerObject == other.gameObject)
                {
                    deathRunGameLoop.ReachedGoal(player.playerId);
                    return;
                }
            }
        }
    }
}