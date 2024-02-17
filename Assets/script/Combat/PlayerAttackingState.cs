using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;

    private bool alreadyAppliedForce;

    private Attack attack;
    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        //to read data that has pass in
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(attack.Damage, attack.Knockback);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }


    public override void Tick(float deltaTime)
    {
        //let the player still experience forces when attacking
        Move(deltaTime);

        FaceTarget();

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

        if (normalizedTime >= previousFrameTime && normalizedTime < 1f) 
        {
            if(normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }

            //if player is trying to attack 
            if (stateMachine.InputReader.IsAttacking) 
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            //go back to free look or targeting state
            //if the player got a target
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                //if not
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));   
            }

        }
        previousFrameTime = normalizedTime;
    }
    public override void Exit()
    {
        
    }

    private void TryApplyForce()
    {
        //to stop player flaying off the map
        if (alreadyAppliedForce) { return; }
        //take in how much force we want to addand in what direction
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);
        alreadyAppliedForce = true;
    }

    private void TryComboAttack(float normalizedTime)
    {
        //if we don't have a combo (make sure we have a combo attack)
        if (attack.ComboStateIndex == -1) { return; }

        //if we can combo (make sure we are far enough to do it)
        if (normalizedTime < attack.ComboAttackTime) { return; }

        //if we are, switch state to attack
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attack.ComboStateIndex));
    }

}
