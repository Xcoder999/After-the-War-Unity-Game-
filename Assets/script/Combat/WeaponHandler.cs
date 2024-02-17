using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject weaponLogic;

    //to turn the weapon logic on when attacking an off when it's not
    //turn on 
    public void EnableWeapon()
    {
        
        weaponLogic.SetActive(true);
    }
    //turn off
    public void DisableWeapon()
    {
        //turn on
        weaponLogic.SetActive(false);
    }
}
