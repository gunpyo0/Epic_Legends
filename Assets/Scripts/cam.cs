using UnityEngine;

public class SmoothFollowCam : MonoBehaviour
{
    [Header("essence")]
    [SerializeField] Transform target;          // 따라갈 대상
    [Header("adjustable ")]
    [SerializeField] float followSpeed = 5f;    // 숫자를 키우면 더 빠르게 수렴
    [SerializeField] float velYInfluence = 0.25f; // velocity.y가 카메라 Y에 반영되는 비율

    Vector3 moveVel = Vector3.zero;             // SmoothDamp용 내부 속도

    void FixedUpdate()
    {
        if (!target) return;

        // 1) 기본 목표 위치
        Vector3 desired = target.position;

        // 2) 대상의 Rigidbody2D에서 velocity.y를 읽어 살짝 위로 보정
        if (target.TryGetComponent(out Rigidbody2D rb))
        {
            desired.y += rb.velocity.y * velYInfluence;
        }

        // 3) 카메라의 z 고정
        desired.z = transform.position.z;

        // 4) SmoothDamp로 부드럽게 이동 (followSpeed가 높을수록 더 빠르게)
        transform.position = Vector3.SmoothDamp(
            transform.position, desired,
            ref moveVel,
            1f / followSpeed
        );
    }
}