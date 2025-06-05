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
    // for timer
    private bool onMoving;
    private float movingCounter;
    /*[SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float startInertiaTime = 0.5f;
    [SerializeField] private float inertiaTimer = 0f;*/

    [Header("Jump")]
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float maxJumpInput = 0.6f;
    [SerializeField] private float jumpForce;
    [SerializeField] private AnimationCurve jumpCurve;

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

    [Header("GameObject")]
    [SerializeField] private GameObject fireRotation;

    [Header("Raycast")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask semiSolidLayer;
    private RaycastHit2D groundHit;
    private RaycastHit2D semiSolidHit;
    private bool isGround = false;
    

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private bool isCoyote = false;

    private CapsuleCollider2D capsuleCollider;
    private Rigidbody2D playerRigidbody;

    private WaitForSeconds coyoteDuration;
    private WaitForSeconds firstJumpDuration;
    private const float FIRST_JUMP_TIME = 0.1f;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        coyoteDuration = new WaitForSeconds(coyoteTime);
        firstJumpDuration = new WaitForSeconds(FIRST_JUMP_TIME);
    }

    private void Update()
    {
        CheckGround();
        //Move();
        newMove();
        JumpInput();
        JumpAction();
    }

    private void CheckGround()
    {
        float rayLength = capsuleCollider.bounds.extents.y + 0.05f;

        groundHit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);
        
        Debug.DrawRay(transform.position, Vector2.down * rayLength, Color.red, 1f);
        isGround = groundHit.collider != null;

        if (isGround && !isFirstJumping) jumpCount = 0;
        if (!isGround && jumpCount == 0 && !isCoyote)
        {
            jumpCount = 0;
            Debug.Log("1");
            StartCoroutine(Coyote());
        }
    }

    void newMove()
    {
        float inputH = Input.GetAxisRaw("Horizontal");

        onMoving = inputH == moveDir;

        if (isGround)
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
            playerRigidbody.AddForce(Vector2.right * inputH * maxSpeed * airRes, ForceMode2D.Force);
            playerRigidbody.velocity = new Vector2(Mathf.Clamp(playerRigidbody.velocity.x, -maxSpeed, maxSpeed), playerRigidbody.velocity.y);
        }
        moveDir = inputH;
    }

    IEnumerator FirstJump() // 처음에 점프 뛸 때 레이길이 콜라이더보다 좀 더 길게 줘서 연속점프가 가능하기에 만든 코루틴함수임 더 나은 방법 있으면 그걸로 ㄱ
    {
        isFirstJumping = true;

        yield return firstJumpDuration;

        isFirstJumping = false;
    }

    IEnumerator Coyote()
    {
        isCoyote = true;

        yield return coyoteDuration;

        isCoyote = false;
    }

    /*private void Move()
    {
        float inputH = Input.GetAxisRaw("Horizontal");

        bool playerMoving = inputH != 0;
        if (playerMoving)
        {
            FireRotate(inputH);
            Inertia();
            playerRigidbody.velocity = new Vector2(inputH * moveSpeed, playerRigidbody.velocity.y);
        }
        else
        {
            if(isGround)
                playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
            inertiaTimer = 0f;
            moveSpeed = maxSpeed / 2;
        }
    }


    private void Inertia()
    {
        inertiaTimer += Time.deltaTime;
        if(inertiaTimer <= startInertiaTime)
        {
            moveSpeed = Mathf.Lerp(maxSpeed / 2, maxSpeed, inertiaTimer);
        }else
        {
            inertiaTimer = 0.5f;
            moveSpeed = maxSpeed;
        }
    }*/


    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
        if (jumpBufferCounter > 0 && (jumpCount < maxJumpCount) && jumpCooldownTimer >= jumpCooldown)
        {

            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
            playerRigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

            // for jump range system
            onJumping = true;

            jumpCount++;
            if (jumpCount == 1) StartCoroutine(FirstJump());
            jumpBufferCounter = 0;
            isCoyote = false;
        }

        // if jump action is triggered and player still holding key
        if (onJumping)
        {
            jumpCounter += Time.deltaTime;
        }
        
        // if at the last moment of pressing the key
        else if(jumpCounter != 0)
        {
            var arrangedJumpCounter = Mathf.Clamp(jumpCounter, 0, maxJumpInput);
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.y * jumpCurve.Evaluate(arrangedJumpCounter / maxJumpInput));
            jumpCounter = 0;
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

//private void IsGround()
//{ 
//    hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);
//    Debug.DrawRay(transform.position, Vector2.down * rayLength, Color.red, 1);
//    isGround = hit.collider != null;

//    if (isGround)
//    {
//        jumpCount = 0;
//    }
//}

//private void Jump()
//{

//    if (Input.GetKeyDown(KeyCode.Space))
//    {
//        if (jumpCount >= 2) return;
//        jumpCount++;
//        playerRigidbody.velocity = Vector3.zero;
//        playerRigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
//    }
//}