using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerSheet", menuName = "PlayerSheet")]
public class SO_PlayerSheet : ScriptableObject
{
    [SerializeField] public float jumpStrength;
    [SerializeField] public float groundForce;
    [SerializeField] public float groundMaxSpeed;
    [SerializeField] public float groundMaxSprintSpeed;
    [SerializeField] public float groundMoveSmoothTime;
    [SerializeField] public float groundNonMoveSmoothTime;
    [SerializeField] public float airForce;
    [SerializeField] public float airMaxSpeed;
    [SerializeField] public float airMaxSprintSpeed;
    [SerializeField] public float airMoveSmoothTime;
    [SerializeField] public float airNonMoveSmoothTime;
    [SerializeField] public float cameraRotateSmoothTime;
    [SerializeField] public float cameraTranslateSmoothTime;

}
