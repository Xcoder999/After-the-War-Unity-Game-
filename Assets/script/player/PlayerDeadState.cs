using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        //turn on Ragdoll
        stateMachine.Ragdoll.ToggleRagdoll(true);
        //turn off weapon
        stateMachine.Weapon.gameObject.SetActive(false);
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        
    }
}
