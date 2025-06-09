using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundCheckBox : MonoBehaviour
{
    [Header("지면 체크용 위치")]
    [SerializeField] private Transform groundCheckPoint;

    [Header("박스 크기 및 오프셋 & 레이어")]
    [SerializeField] private Vector2 boxSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private Vector3 boxOffset = new Vector2(0f, 0f);

    [Tooltip("지면으로 간주할 레이어")]
    [SerializeField] private LayerMask groundLayer;

    /// <summary>지면에 닿아 있으면 true</summary>
    public bool IsGrounded { get; private set; }

    void FixedUpdate()
    {
        // groundCheckPoint 위치를 중심으로 boxSize 크기의 박스 영역에 groundLayer가 있으면 true
        Collider2D hit = Physics2D.OverlapBox(
            groundCheckPoint.position+ boxOffset,
            boxSize,
            0f,
            groundLayer
        );
        IsGrounded = hit != null;
    }

    // 씬 뷰에서 체크 영역 시각화
    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheckPoint.position+ boxOffset, boxSize);
    }
}