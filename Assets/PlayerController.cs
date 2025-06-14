using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour
{

    public static PlayerController now;
    public Rigidbody2D rigid;

    [Header("key setting")]
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;

    private KeyCode lastUsedMoveKey;

    public enum KeyState{
        up,
        down,
        left,
        right,
        firstPressJump,
        jump,
        ignoreSemiGround,
    }

    public class KeyManager
    {
        public List<KeyState> keyState;
        public KeyCode keyCode;
        public KeyManager()
        {
            keyState = new List<KeyState>();
        }
        public void setKeyState(KeyState state)
        {
            keyState.Add(state);
        }
        public void resetKeyState()
        {
            keyState.Clear();
        }

        public bool get(KeyState state)
        {
            return keyState.Contains(state);
        }
    }

    public KeyManager kM = new KeyManager();

    [Header("GameObject & Script")]
    [SerializeField] private GameObject fireRotation;
    [SerializeField] private Collider2D col;

    [Header("ignore semi solid")]
    [SerializeField] private float ignoreTime;

    private CapsuleCollider2D capsuleCollider;
    private WaitForSeconds coyoteDuration;
    
    private void calcInput()
    {
        // reset key state
        kM.resetKeyState();

        // movement
        if(Input.GetKey(leftKey) && Input.GetKey(rightKey))
        {
            if (lastUsedMoveKey != leftKey)
                kM.setKeyState(KeyState.left);
            else
                kM.setKeyState(KeyState.right);
        }
        else if (Input.GetKey(leftKey))
        {
            lastUsedMoveKey= leftKey;
            kM.setKeyState(KeyState.left);
        }
        else if (Input.GetKey(rightKey))
        {
            lastUsedMoveKey = rightKey;
            kM.setKeyState(KeyState.right);
        }

        // IGnore semi ground
        if (Input.GetKey(downKey) && Input.GetKeyDown(jumpKey) && GroundCheckBox.now.isSemiGrounded)
        {
            kM.setKeyState(KeyState.ignoreSemiGround);
        }
        // jump
        else if (Input.GetKeyDown(jumpKey))
        {
            kM.setKeyState(KeyState.jump);
            kM.setKeyState(KeyState.firstPressJump);
        }
        else if (Input.GetKey(jumpKey))
        {
            kM.setKeyState(KeyState.jump);
        }
    }

    void calcSemiGroundIgnore()
    {
        

        if (kM.get(KeyState.ignoreSemiGround))
        {
            StartCoroutine(colliderSetting());

        }
    }

    IEnumerator colliderSetting()
    {

        col.enabled = false;
        yield return new WaitForSeconds(ignoreTime);

        col.enabled = true;
    }

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        // check input
        calcInput();

        // semi ground ignore
        calcSemiGroundIgnore();

        // jump action should be called before move action
        playerJumpManager.now.calcJump();

        //move action
        playerMoveManager.now.calcMove();
    }



    private void Awake()
    {
        if (now == null)
        {
            kM = new KeyManager();
            now = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
