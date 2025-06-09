using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private KeyCode leftKey;
    [SerializeField] private KeyCode rightKey;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float timeToMaxSpeed;
    [SerializeField] private AnimationCurve moveCurve;
    [SerializeField] private float airRes;
    private float moveDir = 0;
    private KeyCode lastUsedKey;
    // for timer
    private bool onMoving;
    private float movingCounter;

    [Header("Jump")]
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;
    [SerializeField] private float addedPower = 2; // jump time for curve evaluation
    [SerializeField] private AnimationCurve jumpCurve;
    private bool isJumpingFrame=false;

    [Header("Multiple Jump")]
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private float jumpCooldown = 0.1f;
    [SerializeField] int jumpCount = 0;
    private bool isFirstJumping = false;
    private float jumpCooldownTimer;
    private float jumpBufferCounter;

    // for timer
    private bool onJumping;
    private float jumpCounter;

    [Header("GameObject & Script")]
    [SerializeField] private GameObject fireRotation;
    [SerializeField]  private GroundCheckBox groundchecker;


    private CapsuleCollider2D capsuleCollider;
    private Rigidbody2D playerRigidbody;

    private WaitForSeconds coyoteDuration;
    private WaitForSeconds firstJumpDuration;
    private const float FIRST_JUMP_TIME = 0.1f;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        firstJumpDuration = new WaitForSeconds(FIRST_JUMP_TIME);
    }

    private void Update()
    {
        // jump action should be called before move action
        JumpInput();
        JumpAction();

        //move action
        Move();
    }

    void Move()
    {
        int movementInput=0;
        // revise manipluation comfortability
        if(Input.GetKey(leftKey) && Input.GetKey(rightKey)){
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
        if(Input.GetKeyDown(leftKey))
            lastUsedKey = leftKey;
        else if (Input.GetKeyDown(rightKey))
            lastUsedKey = rightKey;

        onMoving = movementInput == moveDir;


        // actual velocity changes
        if (groundchecker.IsGrounded)
        {
            

            if (onMoving)
            {
                movingCounter += Time.deltaTime;
            }
            else
            {
                movingCounter = 0;
            }

            var t = Mathf.Clamp(movingCounter / timeToMaxSpeed, 0, 1);
            var speed = moveCurve.Evaluate(t) * maxSpeed;
            playerRigidbody.velocity = new Vector2(moveDir * speed, playerRigidbody.velocity.y);
        }
        else
        {
            if (isJumpingFrame)
            {
                playerRigidbody.velocity = new Vector2(moveDir * maxSpeed, playerRigidbody.velocity.y);
            }
            else
            {
                playerRigidbody.AddForce(Vector2.right * movementInput * maxSpeed * airRes, ForceMode2D.Force);
            }
            playerRigidbody.velocity = new Vector2(Mathf.Clamp(playerRigidbody.velocity.x, -maxSpeed, maxSpeed), playerRigidbody.velocity.y);
        }
        moveDir = movementInput;
    }


    private void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            jumpBufferCounter = jumpBufferTime;
            jumpCooldownTimer = 0;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if(!Input.GetKey(KeyCode.Space))
            onJumping = false;

        jumpCooldownTimer += Time.deltaTime;
    }

    private void JumpAction()
    {

        if (groundchecker.IsGrounded)
        {
            jumpCount = 0;
        }
        if (jumpBufferCounter > 0 && (jumpCount < maxJumpCount) && jumpCooldownTimer >= jumpCooldown)
        {

            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
            playerRigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

            // for jump range system
            onJumping = true;

            isJumpingFrame = true;
            jumpCount++;
            jumpBufferCounter = 0;
            jumpCounter = 0;

            // particle
            ParticleManager.Play("jump", transform.position);
        }
        else
        {
            isJumpingFrame = false;
        }

        // if jump action is triggered and player still holding key
        if (onJumping)
        {
            if (playerRigidbody.velocity.y < 0)
                onJumping = false;
            jumpCounter += Time.deltaTime;
            playerRigidbody.AddForce(transform.up * jumpForce * jumpCurve.Evaluate(Mathf.Clamp(jumpCounter/ jumpTime,0,1)) * addedPower, ForceMode2D.Force);
        }

        // if at the last moment of pressing the key
        else if (jumpCounter != 0)
        {

        }

    }

    private void FireRotate(float inputH)
    {
        switch (inputH)
        {
            case > 0:
                fireRotation.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case < 0:
                fireRotation.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
        }

    }
}
