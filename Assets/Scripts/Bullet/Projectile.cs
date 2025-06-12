using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private bool isPlayerShoot = false;
    [SerializeField] private Item color;
    
    public bool IsPlayerShoot { set { isPlayerShoot = value; } }
    public float Damage { set { damage = value; } }

    // Start is called before the first frame update
    void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Move()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isPlayerShoot) return;
            collision.GetComponent<PlayerHp>().TakeDamage(damage);
            ObjectPoolingManager.ReturnObject(this);
            Debug.Log("플레이어 닿음");
        }
        else if (collision.CompareTag("Enemy"))
        {
            if (!isPlayerShoot) return;
            collision.GetComponent<EnemyHp>().TakeDamage(damage);
            collision.GetComponent<Reward>().LastFragColor = color;
            isPlayerShoot = false;
            ObjectPoolingManager.ReturnObject(this);
        }
        else
        {
            if (collision.GetComponent<Projectile>() != null) return;
            Debug.Log("1");
            ObjectPoolingManager.ReturnObject(this);
        }
    }
}
