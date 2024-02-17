using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ad to Weapon Logic
public class WeaponDamage : MonoBehaviour
{
    /*add a collider so the weapon would do damage to the
     player when collide with enemry (link to player componlent)*/
    [SerializeField] private Collider myCollider;

    private int damage;

    private float knockback;

    //we only want to do one damage to the enermy once every swing not as long as the collider is collide with the enermy
    private List<Collider> alreadyCollidedwith = new List<Collider>();
    private void OnEnable()
    {
        alreadyCollidedwith.Clear();
        Debug.Log($"{myCollider.gameObject.name} WeaponDamage.OnEnable(), Clearing alreadyCollidedWith");
    }

    private void OnTriggerEnter(Collider other)
    {
        //when collide with player
        if (other == myCollider) { return; }

        //make sure it's not on the already collided list
        if(alreadyCollidedwith.Contains(other)) 
        {
            Debug.Log($"{myCollider.gameObject.name} OnTriggerEnter({other.gameObject.name}) has already been hit!");
            return; 
        }
        Debug.Log($"{myCollider.gameObject.name} OnTriggerEnter({other.gameObject.name}) HIT!  Adding to already collided list");

        //if it is not on the Already collided with list
        alreadyCollidedwith.Add(other);
        //if the collider collide if an object with heath script (compolent) eqquiped
        if(other.TryGetComponent<Health>(out Health health))
        {
            Debug.Log($"{myCollider.gameObject.name} OnTriggerEnter({other.gameObject.name}) damaging {health} for {damage}");
            //to deal damage
            health.DealDamage(damage);
        }
        else Debug.Log($"{myCollider.gameObject.name} OnTriggerEnter({other.gameObject.name}) has no Health, not damaging");

        //Apply force when attacked
        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver)) 
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * knockback);
        }
    }

    public void SetAttack(int damage, float knockback)
    { 
        this.damage = damage;
        this.knockback = knockback;
    }


}
