using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State
{
    Search,
    Found,
}

public class Sensor : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private State currenteState;

    public State CurrentState => currenteState;
    private RaycastHit2D playerHit;

    [SerializeField] private float rayRange = 10f;
    [SerializeField] private Transform playerTransform;
    public Transform PlayerTransform => playerTransform;

    private bool isCooldown = false;
    private WaitForSeconds attackDelay;

    private EnemyAttack enemyAttack;

    void Start()
    {
        enemyAttack = GetComponent<EnemyAttack>();
        StartCoroutine(Search());
    }

    void Update()
    {
        playerHit = Physics2D.Raycast(transform.position, (playerTransform.position - transform.position).normalized, rayRange, playerLayer);
        Debug.DrawRay(transform.position, (playerTransform.position - transform.position).normalized * rayRange, Color.blue, 1f);
    }
    private void ChangeState(State state)
    {

        StopCoroutine(currenteState.ToString());
        
        currenteState = state;

        StartCoroutine(currenteState.ToString());
    }

    IEnumerator Search()
    {
        while (true)
        {
            if(playerHit.collider != null)
            {
                Debug.Log("1");
                ChangeState(State.Found);
            }

            yield return null;
        }
    }


    IEnumerator Found()
    {
        while (true)
        {
            if(playerHit.collider != null)
            {
                enemyAttack.Attack();
            }
            else if(playerHit.collider == null)
            {
                Debug.Log("2");
                ChangeState(State.Search);
            }

            yield return null;
        }
    }
}
