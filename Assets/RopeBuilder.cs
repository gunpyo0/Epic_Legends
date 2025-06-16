using UnityEngine;

/// <summary>
/// ���� ����(�� ��ũ��Ʈ�� ���� ������Ʈ)����
/// segmentCount ���� ���� ���� + �� ��(End Mass) ��
/// ��Ÿ�ӿ� �ڵ� �����Ѵ�.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class RopeBuilder : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject segmentPrefab;   // ���� ���� ��� (BoxCollider2D, Rigidbody2D, HingeJoint2D ����)
    public GameObject endMassPrefab;   // �������� �޸� ��(��/�׸� �ƹ��ų�)

    [Header("Rope Settings")]
    public int segmentCount = 15;     // ���� ����
    public float segmentLength = 0.2f;   // ���� ����
    public float segmentMass = 0.1f;   // �� ���� ����
    public float endMass = 2f;     // �� ���� (���� �ٲٸ� �ⷷ���� ����~)

    void Start()
    {
        // �� ������: �� ������Ʈ�� Rigidbody2D (�밳 Kinematic)
        Rigidbody2D prevBody = GetComponent<Rigidbody2D>();
        Vector2 piv = transform.position;

        // �� ���� ������ ����
        for (int i = 0; i < segmentCount; i++)
        {
            Vector2 pos = piv - Vector2.up * segmentLength * (i + 1);
            prevBody = SpawnLink(segmentPrefab, pos, prevBody, segmentMass);
        }

        // �� �� �� ����
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
        hj.enableCollision = false;           // �������� �浹 �� (�ʿ�� �ѵ� OK)

        return rb;
    }
}