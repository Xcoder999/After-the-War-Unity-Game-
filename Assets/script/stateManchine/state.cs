using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        //To get the current animator information
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo =animator.GetNextAnimatorStateInfo(0);

        //to check if the animator is currently transitioning
        //if we were transitioning to and attack we want to get the data for next state
        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            //get the next info, how far through that state we are
            return nextInfo.normalizedTime;
            //if we are not doing any transition and currently playing an attack animation
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
            //if something goes wrong or neither of these are true
        }
        else
        {
            return 0f;
        }
    }
}