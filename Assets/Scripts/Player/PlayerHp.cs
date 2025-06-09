using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{
    [SerializeField] private float maxHp;
    private float currentHp;

    private SpriteRenderer spriteRenderer;
    private bool isDead;

    private WaitForSeconds effectDuration;
    private const float effectDurationTime = 0.5f;

    private void Start()
    {
        currentHp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        effectDuration = new WaitForSeconds(effectDurationTime);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHp -= damage;
        StartCoroutine(Effect());
        if(currentHp <= 0)
        {
            isDead = true;
            currentHp = 0;
            Die();
        }
    }

    IEnumerator Effect()
    {
        spriteRenderer.color = Color.red;

        yield return effectDuration;

        spriteRenderer.color = Color.white;

    }

    public void Heal(float healValue)
    {
        if (isDead) return;
        currentHp += healValue;
    }

    public void Die()
    {
        Debug.Log("Á×À½");
    }
    
}
