using UnityEngine;

/// <summary>
/// FireflyBehaviourSmooth – 타깃 주변을 몽글몽글‥
///  • 반경 안 = Roam (느림) / 밖 = Chase (빠름)
///  • 현재 최고 속도(currentSpeed)가 roam/chase 값을 부드럽게 따라감
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class FireflyBehaviourSmooth : MonoBehaviour
{
    [Header("Target & Area")]
    public Transform center;
    Transform realCenter;
    public float initRadius = 3f;
    public float hysteresis = 0.5f;   // 상태 튕김 방지

    [Header("Speed")]
    public float roamSpeed = 1.2f;   // 반경 안
    public float chaseSpeed = 3.0f;   // 반경 밖
    public float speedBlendRate = 5f; // 속도 전환(m/s²)
    public float steeringAccel = 8f; // 가속 한계 (Rigidbody용)

    public float changeSpeed;

    [Header("Movement Noise")]
    public float noiseScale = 1f;
    public float verticalBob = 0.4f;

    [Header("internal use")]
    public Rigidbody2D rb;
    public InspectorLightManager2D inspectorLightM;

    // ───────────────────────────────────────
    enum State { Roam, Chase }
    State state = State.Roam;

    float currentSpeed;         // ★ 부드럽게 변하는 최고 속도
    float currentRadius;      // 현재 반경
    Vector3 seedPos;
    float seedFlick;

    bool usePhysics;

    void Awake()
    {

        seedPos = new Vector3(Random.value * 10f, Random.value * 10f, Random.value * 10f);
        seedFlick = Random.value * 10f;

        currentSpeed = roamSpeed;    // 시작은 느린 속도
        SetCenter(center, CenterType.main);
    }

    void FixedUpdate()
    {

        // 1) 상태 결정
        float dist = Vector3.Distance(GetPos(), realCenter.position);
        if (state == State.Roam && dist > currentRadius + hysteresis) state = State.Chase;
        else if (state == State.Chase && dist <= currentRadius) state = State.Roam;

        // 2) this frame 목표 속도(targetSpeed) 계산
        float targetSpeed = (state == State.Roam) ? roamSpeed : chaseSpeed;

        // 3) currentSpeed 를 targetSpeed 쪽으로 부드럽게 이동
        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            speedBlendRate * Time.fixedDeltaTime
        );

        // 4) 이번 프레임 목적지
        Vector3 wanted = (state == State.Roam)
            ? realCenter.position + GetRoamOffset(Time.time)
            : realCenter.position;

        // 5) 이동
        MoveTowards(wanted, currentSpeed);

    }

    /*────────── Movement helpers ──────────*/
    Vector3 GetRoamOffset(float t)
    {
        Vector3 dir = new Vector3(
            Mathf.PerlinNoise(seedPos.x, t * noiseScale) - 0.5f,
            Mathf.PerlinNoise(seedPos.y, t * noiseScale) - 0.5f,
            Mathf.PerlinNoise(seedPos.z, t * noiseScale) - 0.5f
        ).normalized;

        Vector3 off = dir * currentRadius;
        off.y += Mathf.Sin(t * 2f + seedPos.x) * verticalBob;
        return off;
    }

    void MoveTowards(Vector2 targetPos, float maxSpeed)
    {
        Vector2 desiredVel = (targetPos - rb.position).normalized * maxSpeed;
        if(desiredVel.magnitude <1)
            desiredVel = Vector2.one* maxSpeed;

        Vector2 steering = Vector2.ClampMagnitude(desiredVel - rb.velocity, steeringAccel);
        rb.AddForce(steering * changeSpeed, ForceMode2D.Force);
    }

    Vector3 GetPos() => usePhysics ? rb.position : transform.position;

    public enum CenterType {
        main,
        target
    }

    public void SetCenter(Transform newCenter, CenterType type)
    {
        if (!newCenter) realCenter = center;
        else
            realCenter = newCenter;

        if(type == CenterType.target)
        {
            currentRadius = initRadius;
        }
        else
        {
            currentRadius = initRadius/2f;
        }

        inspectorLightM.setColor(type);
    }

    void OnDrawGizmosSelected()
    {
        if (center)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(center.position, initRadius);
        }
    }
}
