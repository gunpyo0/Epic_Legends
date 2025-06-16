using UnityEngine;

/// <summary>
/// 시작 지점(이 스크립트를 붙인 오브젝트)부터
/// segmentCount 개의 로프 조각 + 끝 추(End Mass) 를
/// 런타임에 자동 생성한다.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class RopeBuilder : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject segmentPrefab;   // 얇은 막대 모양 (BoxCollider2D, Rigidbody2D, HingeJoint2D 포함)
    public GameObject endMassPrefab;   // 마지막에 달릴 추(원/네모 아무거나)

    [Header("Rope Settings")]
    public int segmentCount = 15;     // 조각 개수
    public float segmentLength = 0.2f;   // 조각 길이
    public float segmentMass = 0.1f;   // 각 조각 질량
    public float endMass = 2f;     // 추 질량 (여길 바꾸면 출렁임이 쭉쭉~)

    void Start()
    {
        // ① 시작점: 이 오브젝트의 Rigidbody2D (대개 Kinematic)
        Rigidbody2D prevBody = GetComponent<Rigidbody2D>();
        Vector2 piv = transform.position;

        // ② 로프 조각들 생성
        for (int i = 0; i < segmentCount; i++)
        {
            Vector2 pos = piv - Vector2.up * segmentLength * (i + 1);
            prevBody = SpawnLink(segmentPrefab, pos, prevBody, segmentMass);
        }

        // ③ 끝 추 생성
        Vector2 endPos = piv - Vector2.up * segmentLength * (segmentCount + 1);
        SpawnLink(endMassPrefab, endPos, prevBody, endMass);
    }

    Rigidbody2D SpawnLink(GameObject prefab, Vector2 pos, Rigidbody2D connectTo, float mass)
    {
        var go = Instantiate(prefab, pos, Quaternion.identity, transform);
        var rb = go.GetComponent<Rigidbody2D>();
        var hj = go.GetComponent<HingeJoint2D>();

        rb.mass = mass;

        hj.connectedBody = connectTo;
        hj.autoConfigureConnectedAnchor = false;
        hj.anchor = new Vector2(0, segmentLength * 0.5f);
        hj.connectedAnchor = new Vector2(0, -segmentLength * 0.5f);
        hj.enableCollision = false;           // 조각끼리 충돌 끔 (필요시 켜도 OK)

        return rb;
    }
}