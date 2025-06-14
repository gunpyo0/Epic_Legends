using UnityEngine;

public class SmoothFollowCam : MonoBehaviour
{
    [Header("essence")]
    [SerializeField] Transform target;          // ���� ���
    [Header("adjustable ")]
    [SerializeField] float followSpeed = 5f;    // ���ڸ� Ű��� �� ������ ����
    [SerializeField] float velYInfluence = 0.25f; // velocity.y�� ī�޶� Y�� �ݿ��Ǵ� ����

    Vector3 moveVel = Vector3.zero;             // SmoothDamp�� ���� �ӵ�

    void FixedUpdate()
    {
        if (!target) return;

        // 1) �⺻ ��ǥ ��ġ
        Vector3 desired = target.position;

        // 2) ����� Rigidbody2D���� velocity.y�� �о� ��¦ ���� ����
        if (target.TryGetComponent(out Rigidbody2D rb))
        {
            desired.y += rb.velocity.y * velYInfluence;
        }

        // 3) ī�޶��� z ����
        desired.z = transform.position.z;

        // 4) SmoothDamp�� �ε巴�� �̵� (followSpeed�� �������� �� ������)
        transform.position = Vector3.SmoothDamp(
            transform.position, desired,
            ref moveVel,
            1f / followSpeed
        );
    }
}