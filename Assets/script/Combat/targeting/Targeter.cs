using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    //link to the targeting group
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;

    //referance the camera
    private Camera mainCamera;
    //To make a List of targets in range
    public List<Target> targets = new List<Target>();

    public Target CurrentTarget { get; private set; }

    private void Start()
    {
        mainCamera = Camera.main;
    }
    //A methord where unity will call it when the collider has something enter it
    private void OnTriggerEnter(Collider other)
    {
        //check if the thing that enter the range even a targert, and then save it in to a variable
        //check if the target a null, if it's null it means the object we collided with is not a target
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        //if it's not null(it's a target)
        targets.Add(target);

        //whenever we had a target enter our range we will add it to the list
        target.OnDestroyed += RemoveTarget;
    }
    // when a target has leave the Range (oppsite of OnTriggerEntr)
    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) { return; }
        //when target is out of range
        RemoveTarget(target);


    }

    //Select Target
    public bool SelectTarget()
    {
        //select the target closest to the center of the screen
        //if there's no target
        if (targets.Count == 0) { return false; }

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;
        foreach (Target target in targets)
        {
            //to find out where an object is on our screen
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);
            //Ignore the targets we can't see
            if (!target.GetComponentInChildren<Renderer>().isVisible)
            {
                continue;
            }
            //target to the closest target when there is mulitple of target at the same time
            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            if(toCenter.sqrMagnitude < closestTargetDistance) 
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }

        }
        if(closestTarget == null) { return false; }

        //if there is atleast one
        CurrentTarget = closestTarget;
        //to add targets to your group
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        
        return true;
    }
    //whhen we exit the targeting state
    public void Cancel()
    {
        //to remove target from the group
        if(CurrentTarget == null) { return; }

        //if current target doesn't exist
        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        //when a target goes out of range
        if (CurrentTarget == target)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform) ;
            CurrentTarget = null;
        }

        /*when a current target has just left the range we want 
         to unsubscrible from the event*/
        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }    
}
