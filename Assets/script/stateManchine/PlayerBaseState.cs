using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    { 
        this.stateMachine = stateMachine; 
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }
    protected void Move(Vector3 motion, float deltaTime) 
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    //a method that make the player face the target when in targeting mode
    protected void FaceTarget()
    {
        //to make sure we had a target (if we had no target)
        if(stateMachine.Targeter.CurrentTarget == null) { return; } 

        //if we had a target, we get the vector pointing from us to them
        Vector3 lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        //we don't want the y-axis to effect anything
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }
    //check if we had a target, if yes go to target state if not go to free look state
    protected void ReturnToLocomotion()
    {
        //if we had a target
        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
}
