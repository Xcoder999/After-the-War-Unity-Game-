using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//add to player and enermy
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int health;
    private bool isInvulnerable;

    public event Action OnTakeDamage;

    public event Action OnDie;

    public bool IsDead => health == 0;

    // Start is called before the first frame update
    //to set health to max when start
    private void Start()
    {
        health = maxHealth;
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable; 
    }
    //to deal damage
    public void DealDamage(int damage)
    {
        //make sure the player isn't dead
        if (health == 0) { return; }

        //don't do any damage when blocking
        if (isInvulnerable) { return; }

        //if player's health drop to zero
        health = Mathf.Max(health - damage, 0);

        //do the impact action when damaged
        OnTakeDamage?.Invoke();

        //if player/Enemy health is 0, then die
        if (health ==0)
        {
            OnDie?.Invoke(); 
        }

        Debug.Log(health);
    }
}
