using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

//Add to player as compolent and drag it in to the Player state machine (force receiver section)
public class ForceReceiver : MonoBehaviour
{
    //connect to Player / Enemy Controller by dragging it in inspector
    [SerializeField] private CharacterController controller;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private float drag = 0.3f;

    private float verticalVelocity;

    private Vector3 dampingVelocity;

    private Vector3 impact;
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;
    // to calculate gravity every single frame
    private void Update()
    {
        //check if we were already on the ground
        if(verticalVelocity <0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            //when we were not no the groud, we want to accelerate downwards
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        //add some impact forces
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (agent != null)
        {
            if (impact.sqrMagnitude < 0.2f * 0.2f)
            {
                impact = Vector3.zero;
                agent.enabled = true;
            }
        }

    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        //If it's the enemy (the navmesh agent only works on the enemy)
        if(agent != null) { agent.enabled = false; }
    }

    public void Jump (float jumpForce)
    {
        verticalVelocity += jumpForce;
    }

    public void Reset()
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }
}
