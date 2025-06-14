using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController now;
    public Rigidbody2D rigid;



    [Header("GameObject & Script")]
    [SerializeField] private GameObject fireRotation;


    private CapsuleCollider2D capsuleCollider;
    

    private WaitForSeconds coyoteDuration;
    private WaitForSeconds firstJumpDuration;
    private const float FIRST_JUMP_TIME = 0.1f;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        firstJumpDuration = new WaitForSeconds(FIRST_JUMP_TIME);
    }

    private void Update()
    {
        // jump action should be called before move action
        playerJumpManager.now.calcJump();

        //move action
        playerMoveManager.now.calcMove();
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
