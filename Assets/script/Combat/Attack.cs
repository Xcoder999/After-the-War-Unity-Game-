using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
//this would create a list to store attacks
public class Attack
{
    [field: SerializeField] public string AnimationName { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; }
    //this adds flexibility in adding more attacks and combo in various different ways
    [field: SerializeField] public int ComboStateIndex { get; private set; } = -1;
    [field: SerializeField] public float ComboAttackTime { get; private set; }
    [field: SerializeField] public float ForceTime { get; private set; }
    [field: SerializeField] public float Force { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float Knockback { get; private set; }
}
