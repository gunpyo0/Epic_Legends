using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hp : MonoBehaviour 
{
    [SerializeField] protected float maxHp;
    protected float currentHp;

    protected bool isDead;
    
    protected virtual void Awake()
    {
        currentHp = maxHp;
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHp -= damage;
        if (currentHp <= 0)
        {
            isDead = true;
            currentHp = 0;
            Die();
        }
    }

    public abstract void Die();

}
