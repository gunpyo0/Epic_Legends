using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform firePos;
    [SerializeField] private Transform fireRotate;

    [SerializeField] private float cooldown = 0.5f; // 적합한 값 찾을 시 상수로 ㄱ
    [SerializeField] private bool isCooldown = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isCooldown)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePos.position, fireRotate.rotation);
            bullet.GetComponent<Bullet>().SetUp(fireRotate);
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
    }
}
