using UnityEngine;

/// �÷��̾�� 'Trigger' �浹�� ��
/// �� ó�� �ε����� ������ Impulse�� �ѹ�!
/// �� ���� �ִ� ���ȿ� �� FixedUpdate���� ��¦ ���� �� �ֱ�
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class RopePlayerCollisionManager : MonoBehaviour
{
    [Header("�� ���(����)")]
    [Tooltip("�÷��̾� �ӵ� * X��� = X�� ��")]
    [SerializeField] float xResistance = 0.2f;
    [Tooltip("�÷��̾� �ӵ� * Y��� = Y�� ��")]
    [SerializeField] float yResistance = 0.2f;

    [Header("�÷��̾� �ĺ�")]
    [SerializeField] string playerTag = "Player";   // Player �±װ� �⺻!

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // �Ǽ� ������: Collider�� ������ Trigger ��
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        Rigidbody2D playerRB = other.attachedRigidbody;
        if (playerRB == null) return;

        // ù ������ 'Impulse' �� ��
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

        // �����ִ� ���ȿ� �ε巴�� ���� �� (Frame����)
        Vector2 force = new Vector2(
            playerRB.velocity.x * xResistance,
            playerRB.velocity.y * yResistance);

        rb.AddForce(force * Time.fixedDeltaTime, ForceMode2D.Force);
    }
}