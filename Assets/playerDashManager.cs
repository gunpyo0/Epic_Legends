using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class playerDashManager : MonoBehaviour
{
    [Header("Dash Settings")]
    public FireflyBehaviourSmooth inspectorM;
    public float dashRange = 3f;          // 검색 반경
    public float dashForce = 20f;         // AddForce(Impulse) 크기
    public float dashDuration = 0.15f;       // 대쉬 유지 시간
    public LayerMask dashMask = ~0;          // 검색 레이어

    Rigidbody2D rb;

    bool isDashing;
    float dashTimer;
    Vector2 dashDir;
    dashable currentTarget;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void LateUpdate()
    {
        var (dashobj,dashTrans)= findDashable();

        if(dashobj != null)
        {
            Debug.Log($"Dashable found: {dashobj} at {dashTrans.position}");
            inspectorM.SetCenter(dashTrans, FireflyBehaviourSmooth.CenterType.target);
        }
        else
            inspectorM.SetCenter(transform, FireflyBehaviourSmooth.CenterType.main);

        if (!isDashing && PlayerController.now.kM.get(PlayerController.KeyState.dash) && dashobj != null)
            TryBeginDash(dashobj,dashTrans);
    }

    void FixedUpdate()
    {
        if (!isDashing) return;

        dashTimer += Time.fixedDeltaTime;
        if (dashTimer >= dashDuration)
            EndDash();
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
    void TryBeginDash(dashable best, Transform bestTrans)
    {
       

        // 2) 대상 있으면 Firefly target 갱신 + 대쉬
        if (best != null)
        {

            dashDir = ((Vector2)bestTrans.position - (Vector2)transform.position).normalized;
            rb.AddForce(dashDir * dashForce, ForceMode2D.Impulse);

            isDashing = true;
            dashTimer = 0f;
            currentTarget = best;
        }
    }

    void EndDash()
    {
        rb.velocity = Vector2.zero;
        isDashing = false;

        if (currentTarget != null)
        {
            currentTarget.triggered();
            currentTarget = null;
        }
    }

    /*────────────────────── Gizmos ──────────────────────*/
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, dashRange);
    }
}
