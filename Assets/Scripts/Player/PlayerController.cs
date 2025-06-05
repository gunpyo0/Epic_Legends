using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int jumpCount = 0;
    [SerializeField] private float startInertiaTime = 0.5f;
    [SerializeField] private float inertiaTimer = 0f;
    [SerializeField] private float jumpInputTime = 2f;
    [SerializeField] private bool isFirstJumping = false;
    
    [Header("GameObject")]
    [SerializeField] private GameObject fireRotation;

    [Header("Raycast")]
    [SerializeField] private LayerMask groundLayer;
    private RaycastHit2D hit;
    private bool isGround = false;

    [Header("Jump Buffering")]
    [SerializeField] private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

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
        Move();
        JumpInput();
        JumpAction();
    }

    private void CheckGround()
    {
        float rayLength = capsuleCollider.bounds.extents.y + 0.05f;

        hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * rayLength, Color.red, 1f);
        isGround = hit.collider != null;

        if (isGround && !isFirstJumping) jumpCount = 0;
        if (!isGround && jumpCount == 0 && !isCoyote)
        {
            jumpCount = 0;
            Debug.Log("1");
            StartCoroutine(Coyote());

        }
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

    private void Move()
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
    }


    private void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void JumpAction()
    {
        if (jumpBufferCounter > 0 && (isGround || isCoyote))
        {
            if (jumpCount >= 2) return;

            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
            playerRigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

            jumpCount++;
            if (jumpCount == 1) StartCoroutine(FirstJump());
            jumpBufferCounter = 0;
            isCoyote = false;
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