using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundCheckBox : MonoBehaviour
{
    public static GroundCheckBox now;

    [Header("test use")]
    [SerializeField] bool showGizmo = false;
    [SerializeField] private Collider2D semiGroundCol;

    public Collider2D SemiGroundCol => semiGroundCol;

    [Header("checkpoint")]
    [SerializeField] private Transform CheckPoint;

    [Header("adjusting properties")]
    [SerializeField] private Vector2 GCboxSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private Vector3 GCboxOffset = new Vector2(0f, 0f);

    [SerializeField] private Vector2 UGCboxSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private Vector3 UGCboxOffset = new Vector2(0f, 0f);

    [SerializeField] private Vector2 wallSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private Vector3 wallOffset = new Vector2(0f, 0f);

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask semiGroundLayer;


    public bool IsGrounded;
    public bool isSemiGrounded;
    public bool isrightWall;
    public bool isleftWall;

    void FixedUpdate()
    {
        // check grounded
        Collider2D GChit = Physics2D.OverlapBox(
            CheckPoint.position+ GCboxOffset,
            GCboxSize,
            0f,
            groundLayer
        );
        Collider2D semiGChit = Physics2D.OverlapBox(
            CheckPoint.position + GCboxOffset,
            GCboxSize,
            0f,
            semiGroundLayer
        );

        Collider2D UGChit = Physics2D.OverlapBox(
            CheckPoint.position + UGCboxOffset,
            UGCboxSize,
            0f,
            groundLayer
        );

        IsGrounded = (GChit != null && UGChit == null);
        isSemiGrounded = (UGChit == null && semiGChit != null);

        if (isSemiGrounded)
        {
            semiGroundCol = semiGChit.gameObject.GetComponent<Collider2D>();
        }

        // check collusion with wall
        Collider2D rightWallHit = Physics2D.OverlapBox(
            CheckPoint.position + wallOffset,
            wallSize,
            0f,
            groundLayer
        );
        isrightWall = rightWallHit != null;

        Collider2D leftWallHit = Physics2D.OverlapBox(
            CheckPoint.position + new Vector3(-wallOffset.x, wallOffset.y, 0),
            wallSize,
            0f,
            groundLayer
        );
        isleftWall = leftWallHit != null;
    }

    // 씬 뷰에서 체크 영역 시각화
    void OnDrawGizmosSelected()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(CheckPoint.position + GCboxOffset, GCboxSize);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(CheckPoint.position + UGCboxOffset, UGCboxSize);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(CheckPoint.position + wallOffset, wallSize);
            Gizmos.DrawWireCube(CheckPoint.position + new Vector3(-wallOffset.x, wallOffset.y, 0), wallSize);
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