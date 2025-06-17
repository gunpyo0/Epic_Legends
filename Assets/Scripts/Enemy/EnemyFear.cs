using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFear : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float runDurationValue = 10f;
    private Sensor sensor;
    private Movement movement;
    private Rigidbody2D enemyRigidbody;

    private State currentState;
    private Transform playerTransform;

    private bool isRunning = false;

    private WaitForSeconds runDuration;

    //private void SetUp(State currState, Transform playerTrans)
    //{
    //    currentState = currState;
    //    playerTransform = playerTrans;
    //}

    private void Start()
    {
        runDuration = new WaitForSeconds(runDurationValue);
        sensor = GetComponent<Sensor>();
        movement = GetComponent<Movement>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(sensor.CurrentState == State.Found && !isRunning)
        {
            if(transform.position.x > sensor.PlayerTransform.position.x)
            {
                StartCoroutine(Run(Vector2.right));
            }
            else
            {
                StartCoroutine(Run(Vector2.left));
            }
        }else if(sensor.CurrentState == State.Search)
        {

        }
    }

    IEnumerator Run(Vector2 dir)
    {
        isRunning = true;
        movement.MoveDirection = dir; // 이동방식 교체

        yield return runDuration;

        isRunning = false;
        movement.MoveDirection = Vector2.zero;
    }
}
