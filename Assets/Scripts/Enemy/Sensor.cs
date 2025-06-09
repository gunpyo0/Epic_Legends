using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State
{
    Search,
    Attack
}

public class Sensor : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private State currenteState;
    public State CurrentState => currenteState;
    private RaycastHit2D playerHit;

    private WaitForSeconds attackDelay;
    private float attackCooldown = 1.5f;
    private bool isCooldown = false;

    private EnemyAttack enemyAttack;


    // Start is called before the first frame update
    void Start()
    {
        attackDelay = new WaitForSeconds(attackCooldown);
        enemyAttack = GetComponent<EnemyAttack>();
        StartCoroutine(Search());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Search()
    {
        while (true)
        {
            float distance = Mathf.Infinity;
            playerHit = Physics2D.Raycast(transform.position, Vector2.left, distance, playerLayer);
            
            if(playerHit.collider != null)
            {
                if (!isCooldown)
                {
                    enemyAttack.Attack();
                    StartCoroutine(AttackCooldown());
                }
            }
            
            yield return null;
        }
    }

    IEnumerator AttackCooldown()
    {
        isCooldown = true;

        yield return attackDelay;
        
        isCooldown = false;
    }
}
