using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMoveManager : MonoBehaviour
{
    public static playerMoveManager now;
    public bool wallSliding = false;

    [Header("Movement")]
    [SerializeField] private float maxSpeed;

    [SerializeField] private float accTime = 0.2f;
    [SerializeField] private AnimationCurve accCurve;

    [SerializeField] private float desTime= 0.2f;
    [SerializeField] private AnimationCurve desCurve;

    [SerializeField] private float airRes;
    private float moveDir = 0;
    private float movementInput;
    // for timer
    private bool onMoving;
    private float accTimer;
    private float desTimer;

    private float firstSpeed;
    private float stillVelocity;

    public void calcWallSliding()
    {
        if(((GroundCheckBox.now.isleftWall && movementInput == -1) || (GroundCheckBox.now.isrightWall && movementInput == 1)) && PlayerController.now.rigid.velocity.y<0)
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
        if(PlayerController.now.kM.get(PlayerController.KeyState.left))
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

            if (!onMoving)
            {
                accTimer = 0;
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
