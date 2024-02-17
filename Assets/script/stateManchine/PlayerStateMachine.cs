﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }

    [field: SerializeField] public CharacterController Controller { get; private set; }

    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }

    [field: SerializeField] public Animator Animator { get; private set; }

    public Transform MainCameraTransform { get; private set; }

    [field: SerializeField] public float RotationDamping { get; private set; }

    [field: SerializeField] public Targeter Targeter { get; private set; }

    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }

    [field: SerializeField] public Attack[] Attacks { get; private set; }
    //referance weaponLogic
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }

    [field: SerializeField] public Health Health { get; private set; }

    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public float DodgeDuration { get; private set; }
    [field: SerializeField] public float DodgeLength { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }

    [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; }
    public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //make the camera look for an object with the tag camera
        MainCameraTransform = Camera.main.transform;

        SwitchState(new PlayerFreeLookState(this));
    }
    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }
    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        //when we take damage we want to switch state to the impact state
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleDie()
    {
        //when we take damage we want to switch state to the Dead state
        SwitchState(new PlayerDeadState(this));
    }

}