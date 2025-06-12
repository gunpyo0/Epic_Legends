using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : Hp, IHealable
{
    [SerializeField] private bool isInvinc = false;
    public bool IsInvinc { set { isInvinc = value; } }

    public override void TakeDamage(float damage)
    {
        if (isInvinc) return;
        base.TakeDamage(damage);
    }

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
