using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    //referance the animator blend tree
    private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");

    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");

    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");

    private float remainingDodgeTime;

    private Vector3 dodgingDirectionInput;

    private const float CrossFadeDuration = 0.1f;
    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine)
    {
        this.dodgingDirectionInput = dodgingDirectionInput;
    }

    public override void Enter()
    {
        remainingDodgeTime = stateMachine.DodgeDuration;

        stateMachine.Animator.SetFloat(DodgeForwardHash, dodgingDirectionInput.y);
        stateMachine.Animator.SetFloat(DodgeRightHash, dodgingDirectionInput.x);
        stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);

        stateMachine.Health.SetInvulnerable(true);
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeLength / stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeLength / stateMachine.DodgeDuration;

        Move(movement, deltaTime);

        FaceTarget();

        remainingDodgeTime -= deltaTime;

        if (remainingDodgeTime <= 0)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
    }
}
