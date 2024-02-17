using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyChasingState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //to reach out to the enemy Animator's Locomotion
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);

    }

    public override void Exit() 
    {
        //when we Exit this state
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    public override void Tick(float deltaTime)
    {

        //if it's not in chase range
        if (!IsInChaseRange())
        {
            //switch to idle state
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        else if (IsInAttackRange()) 
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);

        FacePlayer();

        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    private void MoveToPlayer(float deltaTime) 
    {
        //tell the enemy where to go
        if (stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = stateMachine.Player.transform.position;

            //don't move in a stright line (there could be a wall + no speed, veolcity or acceration change + how far we should move
            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }

        //tell the navigation Agent how much we move
        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }

    private bool IsInAttackRange()
    {
        //if player is dead wewere not in attack range (so the enemy would stop attacking when dead)
        if (stateMachine.Player.IsDead) { return false; }

        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        //To know if we were in range our not
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}

