using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerSheet", menuName = "PlayerSheet")]
public class SO_PlayerSheet : ScriptableObject
{
    [SerializeField] public float jumpStrength;
    [SerializeField] public float groundForce;
    [SerializeField] public float groundMaxSpeed;
    [SerializeField] public float groundSmoothTime;
    [SerializeField] public float airForce;
    [SerializeField] public float airMaxSpeed;
    [SerializeField] public float airSmoothTime;
    [SerializeField] public float sensitivity;

}
