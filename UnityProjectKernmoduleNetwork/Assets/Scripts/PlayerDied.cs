using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDied : MonoBehaviour
{
    [SerializeField] private DeathRunGameLoop deathRunGameLoop;

    private void OnTriggerEnter(Collider other)
    {
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            SessionVariables.instance.myGameClient.SendToServer(new Net_PlayerDied(SessionVariables.instance.myPlayerId));
            return;
        }
    }
}
