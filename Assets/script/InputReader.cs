using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public bool IsAttacking {  get; private set; }
    public bool IsBlocking { get; private set; }
    public Vector2 MovementValue {  get; private set; }  

    public event Action JumpEvent;

    public event Action DodgeEvent;

    private Controls controls;

    public event Action TargetEvent;

    public event Action CancelEvent;
    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        //if they didn't press the button, then return
        if(!context.performed) { return; }

        //if they press the button
        TargetEvent?.Invoke(); 
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        //if they didn't press the button, then return
        if (!context.performed) { return; }

        //if they press the button
        CancelEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        //if we press the attack button
        if (context.performed)
        {
            IsAttacking = true;
            //if released
        }
        else if (context.canceled)
        {
            IsAttacking = false;
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        //if we press the blocking button
        if (context.performed)
        {
            IsBlocking = true;
            //if released
        }
        else if (context.canceled)
        {
            IsBlocking = false;
        }
    }
}
