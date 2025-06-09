using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundCheckBox : MonoBehaviour
{
    [Header("���� üũ�� ��ġ")]
    [SerializeField] private Transform groundCheckPoint;

    [Header("�ڽ� ũ�� �� ������ & ���̾�")]
    [SerializeField] private Vector2 boxSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private Vector3 boxOffset = new Vector2(0f, 0f);

    [Tooltip("�������� ������ ���̾�")]
    [SerializeField] private LayerMask groundLayer;

    /// <summary>���鿡 ��� ������ true</summary>
    public bool IsGrounded { get; private set; }

    void FixedUpdate()
    {
        // groundCheckPoint ��ġ�� �߽����� boxSize ũ���� �ڽ� ������ groundLayer�� ������ true
        Collider2D hit = Physics2D.OverlapBox(
            groundCheckPoint.position+ boxOffset,
            boxSize,
            0f,
            groundLayer
        );
        IsGrounded = hit != null;
    }

    // �� �信�� üũ ���� �ð�ȭ
    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheckPoint.position+ boxOffset, boxSize);
    }
}