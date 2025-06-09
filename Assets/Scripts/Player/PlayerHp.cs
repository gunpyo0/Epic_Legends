using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : Hp, IHealable
{

    public void Heal(float healAmount)
    {
        if (isDead) return;
        currentHp += healAmount;
    }

    protected override void Die()
    {
        Debug.Log("Á×À½");
    }
    
}
