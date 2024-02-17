using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int LocomotionHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        //to reach out to the enemy Animator's Locomotion
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);

    }

    public override void Exit(){ }

    public override void Tick(float deltaTime)
    {
        //react to any forces like knock back, attacks...etc
        Move(deltaTime);

        //if it's in chase range
        if (IsInChaseRange())
        {
            //switch to chasing state
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }
        stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
    }
}
