using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundCheckBox : MonoBehaviour
{
    public static GroundCheckBox now;

    [Header("test use")]
    [SerializeField] bool showGizmo = false;

    [Header("checkpoint")]
    [SerializeField] private Transform GCPoint;
    [SerializeField] private Transform UGCPoint;

    [Header("adjusting properties")]
    [SerializeField] private Vector2 GCboxSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private Vector3 GCboxOffset = new Vector2(0f, 0f);

    [SerializeField] private Vector2 UGCboxSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private Vector3 UGCboxOffset = new Vector2(0f, 0f);

    [SerializeField] private LayerMask groundLayer;

    public bool IsGrounded { get; private set; }

    void FixedUpdate()
    {
        // groundCheckPoint 위치를 중심으로 boxSize 크기의 박스 영역에 groundLayer가 있으면 true
        Collider2D GChit = Physics2D.OverlapBox(
            GCPoint.position+ GCboxOffset,
            GCboxSize,
            0f,
            groundLayer
        );

        Collider2D UGChit = Physics2D.OverlapBox(
            UGCPoint.position + UGCboxOffset,
            UGCboxSize,
            0f,
            groundLayer
        );

        IsGrounded = (GChit != null && UGChit == null);
    }

    // 씬 뷰에서 체크 영역 시각화
    void OnDrawGizmosSelected()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(GCPoint.position + GCboxOffset, GCboxSize);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(UGCPoint.position + UGCboxOffset, UGCboxSize);
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