using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMoveManager : MonoBehaviour
{
    public static playerMoveManager now;

    [Header("Movement")]
    [SerializeField] private KeyCode leftKey;
    [SerializeField] private KeyCode rightKey;
    [SerializeField] private float maxSpeed;

    [SerializeField] private float accTime = 0.2f;
    [SerializeField] private AnimationCurve accCurve;

    [SerializeField] private float desTime= 0.2f;
    [SerializeField] private AnimationCurve desCurve;

    [SerializeField] private float airRes;
    private float moveDir = 0;
    private KeyCode lastUsedKey;
    // for timer
    private bool onMoving;
    private float accTimer;
    private float desTimer;

    private float firstSpeed;
    private float stillVelocity;

    public void calcMove()
    {
        int movementInput = 0;
        // revise manipluation comfortability
        if (Input.GetKey(leftKey) && Input.GetKey(rightKey))
        {
            if (leftKey == lastUsedKey)
                movementInput = -1;
            else
                movementInput = 1;
        }
        else if (Input.GetKey(leftKey))
        {
            movementInput = -1;
        }
        else if (Input.GetKey(rightKey))
        {
            movementInput = 1;
        }
        if (Input.GetKeyDown(leftKey))
            lastUsedKey = leftKey;
        else if (Input.GetKeyDown(rightKey))
            lastUsedKey = rightKey;

        onMoving = movementInput == moveDir;


        // actual velocity changes
        if (GroundCheckBox.now.IsGrounded)
        {
            if (accTimer != 0 && moveDir==0)
            {
                firstSpeed = PlayerController.now.rigid.velocity.x;
            }

            //acc
            if (moveDir != 0)
            {
                var t = Mathf.Clamp(accTimer / accTime, 0, 1);
                var speed = accCurve.Evaluate(t) * maxSpeed;
                PlayerController.now.rigid.velocity = new Vector2(moveDir * speed, PlayerController.now.rigid.velocity.y);
            }

            //des
            else
            {
                var t = Mathf.Clamp(desTimer / desTime, 0, 1);
                var speed = desCurve.Evaluate(t);
                PlayerController.now.rigid.velocity = new Vector2(firstSpeed * speed, PlayerController.now.rigid.velocity.y);
                Debug.Log(speed);
            }

            if (onMoving && moveDir !=0)
            {
                accTimer += Time.deltaTime;
                desTimer = 0;

            }
            if (moveDir == 0)
            {
                accTimer = 0;
                desTimer += Time.deltaTime;
            }
        }
        else
        {
            if (playerJumpManager.now.isJumpingFrame && !playerJumpManager.now.firstJump)
            {
                PlayerController.now.rigid.velocity = new Vector2(moveDir * maxSpeed, PlayerController.now.rigid.velocity.y);
            }
            else
            {
                PlayerController.now.rigid.AddForce(Vector2.right * movementInput * maxSpeed * airRes, ForceMode2D.Force);
            }
            PlayerController.now.rigid.velocity = new Vector2(Mathf.Clamp(PlayerController.now.rigid.velocity.x, -maxSpeed, maxSpeed), PlayerController.now.rigid.velocity.y);
        }
        moveDir = movementInput;
    }

    void Awake()
    {
        if (now == null)
        {
            now = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
