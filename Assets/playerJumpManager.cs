using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerJumpManager : MonoBehaviour
{
    public static playerJumpManager now;


    [Header("Jump")]
    [SerializeField] private float jumpBufferTime = 0.1f;
    public bool isJumpingFrame = false;

    [Header("Jump Power")]
    [SerializeField] private float jumpForce;


    [SerializeField] private float originGravity;
    [SerializeField] private float midAirAdjustRange;
    [SerializeField] private float onGravity;

    [Header("Multiple Jump")]
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private float jumpCooldown = 0.1f;
    [SerializeField] int jumpCount = 0;

    public bool firstJump = true;
    private float jumpCooldownTimer;
    private float jumpBufferCounter;
    

    // for timer
    private bool onJumping;

    public void calcJump()
    {
        JumpInput();
        JumpAction();
    }

    private void JumpInput()
    {
        if (PlayerController.now.kM.get(PlayerController.KeyState.firstPressJump))
        {
            jumpBufferCounter = jumpBufferTime;
            jumpCooldownTimer = 0;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (!PlayerController.now.kM.get(PlayerController.KeyState.jump))
            onJumping = false;

        jumpCooldownTimer += Time.deltaTime;
    }

    private void JumpAction()
    {

        if (GroundCheckBox.now.IsGrounded)
        {
            jumpCount = 0;
            jumpCooldownTimer = jumpCooldown;
            firstJump = true;
        }
        if (jumpBufferCounter >= 0 && (jumpCount < maxJumpCount) && (jumpCooldownTimer >= jumpCooldown))
        {

            PlayerController.now.rigid.velocity = new Vector2(PlayerController.now.rigid.velocity.x, 0);
            PlayerController.now.rigid.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

            // for jump range system
            onJumping = true;

            isJumpingFrame = true;
            jumpCount++;
            jumpBufferCounter = 0;
            firstJump = false;

            // particle
            ParticleManager.Play("jump", transform.position);
        }
        else
        {
            isJumpingFrame = false;
        }

        var yVelocity = PlayerController.now.rigid.velocity.y;

        // if jump action is triggered and player still holding key
        if (onJumping)
        {
            if (yVelocity < 0)
                onJumping = false;
        }
        else
        {
            PlayerController.now.rigid.gravityScale = originGravity;
        }

        if ((onJumping && yVelocity >= 0 && firstJump) || (yVelocity <= midAirAdjustRange && yVelocity >= -midAirAdjustRange) || playerMoveManager.now.wallSliding)
        {
            PlayerController.now.rigid.gravityScale = onGravity;
        }
        else
        {
            PlayerController.now.rigid.gravityScale = originGravity;

        }
    }

    private void Awake()
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
