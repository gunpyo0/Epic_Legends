using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private bool isSetupDone = false;
    private Rigidbody2D bulletRigidbody;
    private bool fired = false;

    private Transform firePoint;

    public void SetUp(Transform firePos)
    {
        firePoint = firePos;
        isSetupDone = true;
    }

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isSetupDone || bulletRigidbody == null) return;
        if (fired) return;

        bulletRigidbody.velocity = firePoint.right * speed;
        fired = true;
    }
}
