using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsHandler : MonoBehaviour
{
    private Dictionary<int, Trap> traps = new Dictionary<int, Trap>();

    private void Start()
    {
        SessionVariables.instance.myGameClient.trapsHandler = this;
    }

    public void AddToTraps(int trapId, Trap trap)
    {
        traps.Add(trapId, trap);
    }

    public void ActivateTrap(int trapId)
    {
        traps[trapId].ActivateTrap();
    }
}
