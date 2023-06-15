using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private FiniteStateMachine fsm;
    private BaseState[] states;
    [SerializeField] private BaseState startState;
    [SerializeField] private SessionVariables mySession;

    private void Awake()
    {
        states = GetComponents<BaseState>();
        fsm = new FiniteStateMachine(startState, states);
    }

    private void Update()
    {
        fsm.OnUpdate();
    }

    private void FixedUpdate()
    {
        fsm.OnFixedUpdate();
    }

    private void LateUpdate()
    {
        fsm.OnLateUpdate();
    }
}
