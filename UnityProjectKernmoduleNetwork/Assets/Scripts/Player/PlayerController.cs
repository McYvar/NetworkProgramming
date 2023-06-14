using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private FiniteStateMachine fsm;
    [SerializeField] private BaseState[] states;
    [SerializeField] private BaseState startState;
    [SerializeField] private SessionVariables mySession;

    private void Start()
    {
        fsm = new FiniteStateMachine(startState, states);
    }

    private void Update()
    {
        fsm.OnUpdate();
    }
}
