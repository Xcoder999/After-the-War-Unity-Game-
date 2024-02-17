using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    //set to enemy
    [field: SerializeField] public Animator Animator { get; private set; }

    //connect to Enemy Controller compolent
    [field: SerializeField] public CharacterController Controller { get; private set; }

    //connect to force reciver script
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }

    [field: SerializeField] public float PlayerChasingRange {  get; private set; }

    public Health Player { get; private set; }

    [field: SerializeField] public NavMeshAgent Agent { get; private set; }

    [field: SerializeField] public float MovementSpeed { get; private set; }

    [field: SerializeField] public float AttackRange { get; private set; }

    [field: SerializeField] public WeaponDamage Weapon { get; private set; }

    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: SerializeField] public int AttackKnockback { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }



    private void Start()
    {
        //enemy look for game object with tag player
        Player=GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        //we don't want the actual object to move and route with the agent
        Agent.updatePosition = false;
        Agent.updateRotation = false;

        SwitchState(new EnemyIdleState(this));
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
        SwitchState(new EnemyImpactState(this));
    }

    //range of detecting the player
    private void OnDrawGizmosSelected()
    {
        //draw a range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }

    private void HandleDie()
    {
        //when we take damage we want to switch state to the Dead state
        SwitchState(new EnemyDeadState(this));
    }
}
