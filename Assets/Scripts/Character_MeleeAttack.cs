using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_MeleeAttack : MonoBehaviour
{
    public GameObject weaponHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (GetCurrentHoldingWeapon() != null)
                Attack();
        }
    }

    public GameObject GetCurrentHoldingWeapon()
    {
        GameObject weapon = null;
        if(weaponHolder.transform.childCount > 0)
        {
            weapon = weaponHolder.transform.GetChild(0).gameObject;
        }
        return weapon;
    }

    public void Attack()
    {
        GameObject weapon = GetCurrentHoldingWeapon();
        weapon.GetComponent<Animator>().Play("Sword_Attack");
    }
}
