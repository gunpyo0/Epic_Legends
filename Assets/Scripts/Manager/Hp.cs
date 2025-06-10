using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream
public abstract class Hp : MonoBehaviour 
=======
public abstract class Hp : MonoBehaviour
>>>>>>> Stashed changes
{
    [SerializeField] protected float maxHp;
    protected float currentHp;

    protected SpriteRenderer spriteRenderer;
    protected WaitForSeconds effectDuration;
    protected const float effectDurationTime = 0.25f;

    protected bool isDead;
<<<<<<< Updated upstream
    
=======

>>>>>>> Stashed changes
    protected virtual void Awake()
    {
        currentHp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        effectDuration = new WaitForSeconds(effectDurationTime);
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHp -= damage;
        StartCoroutine(Effect());
        if (currentHp <= 0)
        {
            isDead = true;
            currentHp = 0;
            Die();
        }
    }

    protected virtual IEnumerator Effect()
    {
        Color color = spriteRenderer.color;

        color.a = 0.5f;

        yield return effectDuration;

        color.a = 1f;
    }

    protected abstract void Die();

}
