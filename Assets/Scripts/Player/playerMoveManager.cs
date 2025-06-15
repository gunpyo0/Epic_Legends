using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerMoveManager : MonoBehaviour
{
    public static playerMoveManager now;
    public bool wallSliding = false;

    [Header("Movement")]
    [SerializeField] private float maxSpeed;

    [SerializeField] private float speedToMax;
    [SerializeField] private float airRes;

    [Header("turn")]
    [SerializeField] private float turnSpeed;
    [SerializeField] private float turnTime;
    float turnTimer = 0;
    bool isTurning = false;
    float turnDir = 0;

    private float moveDir = 0;
    private float movementInput;
    // for timer
    private bool onMoving;

    public void calcWallSliding()
    {
        if (((GroundCheckBox.now.isleftWall && movementInput == -1) || (GroundCheckBox.now.isrightWall && movementInput == 1)) && PlayerController.now.rigid.velocity.y < 0)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }
    }

    public void calcMove()
    {
        movementInput = 0;
        if (PlayerController.now.kM.get(PlayerController.KeyState.left))
        {
            movementInput = -1;
        }
        else if (PlayerController.now.kM.get(PlayerController.KeyState.right))
        {
            movementInput = 1;
        }

        // revise manipluation comfortability
        onMoving = movementInput == moveDir;

        // wall sliding calc
        calcWallSliding();

        // actual velocity changes
        if (GroundCheckBox.now.IsGrounded)
        {
            if(isTurning)
                turnTimer += Time.deltaTime;

            if(isTurning && turnTimer >= turnTime)
            {
                isTurning = false;
                turnTimer = 0;
            }

            var nowSpeed = PlayerController.now.rigid.velocity.x;
            var targetSpeed = movementInput * speedToMax;

            PlayerController.now.rigid.AddForce(new Vector2(targetSpeed * (maxSpeed - Mathf.Abs(nowSpeed)) / maxSpeed, 0), ForceMode2D.Force);

            if(isTurning)
            {
                isTurning = false;
                turnTimer = 0;
                PlayerController.now.rigid.AddForce(new Vector2(targetSpeed * turnSpeed, 0), ForceMode2D.Impulse);
            }

            if(moveDir != movementInput && moveDir != 0)
            {
                turnDir = movementInput;
                turnTimer = 0;
                isTurning = true;
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
                var nowSpeed = PlayerController.now.rigid.velocity.x;
                var targetSpeed = movementInput * speedToMax;
                PlayerController.now.rigid.AddForce(new Vector2(targetSpeed * (maxSpeed - Mathf.Abs(nowSpeed)) / maxSpeed, 0), ForceMode2D.Force);
            }
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