using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    //referance the animator blend tree
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");

    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");

    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    private const float CrossFadeDuration = 0.1f;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        /*we want to enter the targeting state by pressing "T" on keyboard, 
            and then stop the targetting stateby hitting "c" on keyboard*/
        /*access the InputReader though state machine.
        When cancel event is Triggered then call the on target mathod*/
        stateMachine.InputReader.CancelEvent += OnCancel;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;

        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Exit()
    {
        //when we stop
        stateMachine.InputReader.CancelEvent -= OnCancel;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    /*when someone hit the targeting key we want it to 
     enter the targeting state*/
    private void OnCancel()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private void OnDodge()
    {
        //if the player isn't pressing any movement keys then we don't do dodging
        if (stateMachine.InputReader.MovementValue == Vector2.zero) { return; }

        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
    }

    private void OnJump()
    {
        //switch state to the Jumping State
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    public override void Tick(float deltaTime)
    {
        /*if trying to attack in Targeting state,
         then we want to switch to the attacking state*/
        if(stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }
        /*if trying to block in Targeting state,
        then we want to switch to the Blocking state*/
        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null)
        {
            //when we lose our target
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement(deltaTime);

        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }
    
    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();


        //to move left and right relative to our target
        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        


        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        //for y axis
        //if we aren't moving
        if(stateMachine.InputReader.MovementValue.y ==0)
        {
            stateMachine.Animator.SetFloat(TargetingForwardHash, 0, 0.1f, deltaTime);
        }
        else
        {
            //if we are moving, if we were moving forward use 1 then use -1
            float value = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingForwardHash, value, 0.1f, deltaTime);
        }
        //for x axis
        //if we aren't moving
        if (stateMachine.InputReader.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(TargetingRightHash, 0, 0.1f, deltaTime);
        }
        else
        {
            //if we are moving, if we were moving forward use 1 then use -1
            float value = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingRightHash, value, 0.1f, deltaTime);
        }
    }
}
