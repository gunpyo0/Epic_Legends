using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공사예정
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileGroup;
    
    private bool isCooldown = false;

    private ObjectPoolingManager poolingManager = ObjectPoolingManager.Instance;
    private Sensor sensor;

    private WaitForSeconds attackCooldownDuration;

    private void Start()
    {
        sensor = GetComponent<Sensor>();
        attackCooldownDuration = new WaitForSeconds(attackCooldown);
    }

    public void Attack()
    {
        if (!isCooldown)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        isCooldown = true;

        yield return attackCooldownDuration;

        isCooldown = false;
    }
}
