using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform firePos;
    [SerializeField] private Transform fireRotate;

    [SerializeField] private float bulletSpeed = 10f;

    [SerializeField] private float damage = 1f;

    [SerializeField] private Transform projectileGroup;

    [SerializeField] private float cooldown = 0.5f; // 적합한 값 찾을 시 상수로 ㄱ
    [SerializeField] private bool isCooldown = false;
    private WaitForSeconds duration;

    void Start()
    {
        duration = new WaitForSeconds(cooldown);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isCooldown)
        {
            var bullet = ObjectPoolingManager.GetObject(projectileGroup);
            bullet.GetComponent<Projectile>().IsPlayerShoot = true;
            bullet.GetComponent<Projectile>().Damage = damage;
            bullet.GetComponent<Movement>().MoveDirection = Vector3.right;
            bullet.GetComponent<Movement>().MoveSpeed = bulletSpeed;
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return duration;
        isCooldown = false;
    }
}
