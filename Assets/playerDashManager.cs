using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class playerDashManager : MonoBehaviour
{
    [Header("Search Settings")]
    public FireflyBehaviourSmooth inspectorM;
    public float dashRange = 3f;          // 검색 반경
    public float velocityPreservedDashRange = 0.5f;          // 검색 반경, 편의성을 위해 어느정도 가까이 있으면 현재 방향으로 진행
    public LayerMask dashMask = ~0;          // 검색 레이어

    [Header("Dash Settings")]
    public float maxDashForce = 20f;
    public float dashDuration = 0.7f; // 대쉬 지속 시간
    public AnimationCurve dashCurve; // 대쉬 파워 곡선

    public float dashCooldown = 1f; // 대쉬 쿨타임
    public float dashTriggerRange = 0.5f; // 대쉬 대상 객체 트리거 범위
    Timer cooldown;

    Rigidbody2D rb;
    playerJumpManager jumpM;

    bool isDashing;
    float dashTimer;
    Vector2 dashPower;

    bool isTriggered;
    dashable currentTarget;
    Transform currentTargetTrans;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpM = GetComponent<playerJumpManager>();
    }

    private void Start()
    {
        cooldown = new Timer(0);
    }

    void LateUpdate()
    {
        var (dashobj,dashTrans)= findDashable();

        if(dashobj != null)
        {
            inspectorM.SetCenter(dashTrans, FireflyBehaviourSmooth.CenterType.target);
        }
        else
            inspectorM.SetCenter(transform, FireflyBehaviourSmooth.CenterType.main);

        if (!isDashing && PlayerController.now.kM.get(PlayerController.KeyState.dash) && dashobj != null && cooldown.check())
        {
            TryBeginDash(dashobj, dashTrans, Vector2.Distance(dashTrans.position, transform.position) <= velocityPreservedDashRange);
        }
            
    }

    void FixedUpdate()
    {
        if (!isDashing) return;
        calcDashForce();
        dashTimer += Time.fixedDeltaTime;

        if (Vector2.Distance(transform.position, currentTargetTrans.position) <= dashTriggerRange && !isTriggered)
        {
            // 대쉬 대상이 멀어지면 대쉬 취소
            currentTarget.triggered(transform.gameObject);
            isTriggered = true;
        }

        if (dashTimer > dashDuration || PlayerController.now.kM.get(PlayerController.KeyState.jump))
        {
            // dash finished
            FinishDash();
        }
    }

    public void dash(Vector2 dashPos, bool preserve)
    {
        isDashing = true;
        dashTimer = 0f;

        var dir =Vector2.zero;
        if (preserve)
        {
            dir = rb.velocity.normalized;
        }
        else
            dir = (dashPos - (Vector2)transform.position).normalized;
        dashPower =  dir * maxDashForce;
    }

    public void calcDashForce()
    {
        var t = dashTimer / dashDuration;
        var multPower = dashCurve.Evaluate(t);
        rb.velocity = dashPower * multPower;

    }

    (dashable, Transform) findDashable()
    {
        // 1) 주변 dashable 탐색
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, dashRange, dashMask);

        float bestDist = float.MaxValue;
        dashable best = null;
        Transform bestPos=null;

        foreach (var col in hits)
        {
            if (col.TryGetComponent(out dashable d))
            {
                float dist = Vector2.Distance(transform.position, col.transform.position);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    best = d;
                    bestPos = col.transform;
                }
            }
        }
        return (best,bestPos);
    }

    /*────────────────────── Dash Logic ──────────────────────*/
    void TryBeginDash(dashable best, Transform bestTrans, bool preserve)
    {

        // 2) 대상 있으면 Firefly target 갱신 + 대쉬
        if (best != null)
        {

            currentTarget = best;
            currentTargetTrans = bestTrans;
            isTriggered = false;

            dash(bestTrans.position, preserve);
        }
    }

    void FinishDash()
    {
        isDashing = false;
        cooldown.reset(dashCooldown);
    }

    /*────────────────────── Gizmos ──────────────────────*/
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, dashRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, velocityPreservedDashRange);
    }
}
