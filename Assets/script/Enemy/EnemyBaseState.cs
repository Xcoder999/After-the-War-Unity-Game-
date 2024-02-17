using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
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

    //To face the player
    protected void FacePlayer()
    {
        //to make sure we had detect a player (if we had no player)
        if (stateMachine.Player == null) { return; }

        //if we had a target, we get the vector pointing from us to them
        Vector3 lookPos = stateMachine.Player.transform.position - stateMachine.transform.position;
        //we don't want the y-axis to effect anything
        lookPos.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    //enemy chase range
    protected bool IsInChaseRange()
    {
        //if player is dead wewere not in attack range (so the enemy would stop attacking when dead)
        if (stateMachine.Player.IsDead) { return false; }

        //get the position of the player then subtract in from our position
        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        //make sure the distance is less than or equal to the chasing range and the we will return to ture our false base on that
        return playerDistanceSqr <= stateMachine.PlayerChasingRange * stateMachine.PlayerChasingRange;
    }
}
