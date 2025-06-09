using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileGroup;
    
    
    public void Attack()
    {
        var bullet = ObjectPoolingManager.GetObject(projectileGroup);
        bullet.GetComponent<Movement>().MoveDirection = Vector3.left;
        bullet.GetComponent<Movement>().MoveSpeed = attackSpeed;
        bullet.GetComponent<Projectile>().Damage = damage;
    }
}
