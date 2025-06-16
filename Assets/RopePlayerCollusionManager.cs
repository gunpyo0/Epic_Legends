using UnityEngine;

/// 플레이어와 'Trigger' 충돌할 때
/// ① 처음 부딪히는 순간엔 Impulse로 한방!
/// ② 겹쳐 있는 동안엔 매 FixedUpdate마다 살짝 힘을 더 주기
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class RopePlayerCollisionManager : MonoBehaviour
{
    [Header("힘 계수(저항)")]
    [Tooltip("플레이어 속도 * X계수 = X축 힘")]
    [SerializeField] float xResistance = 0.2f;
    [Tooltip("플레이어 속도 * Y계수 = Y축 힘")]
    [SerializeField] float yResistance = 0.2f;

    [Header("플레이어 식별")]
    [SerializeField] string playerTag = "Player";   // Player 태그가 기본!

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // 실수 방지용: Collider를 강제로 Trigger 로
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        Rigidbody2D playerRB = other.attachedRigidbody;
        if (playerRB == null) return;

        // 첫 순간엔 'Impulse' 한 번
        Vector2 impulse = new Vector2(
            playerRB.velocity.x * xResistance,
            playerRB.velocity.y * yResistance);

        rb.AddForce(impulse, ForceMode2D.Impulse);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        Rigidbody2D playerRB = other.attachedRigidbody;
        if (playerRB == null) return;

        // 겹쳐있는 동안엔 부드럽게 지속 힘 (Frame마다)
        Vector2 force = new Vector2(
            playerRB.velocity.x * xResistance,
            playerRB.velocity.y * yResistance);

        rb.AddForce(force * Time.fixedDeltaTime, ForceMode2D.Force);
    }
}