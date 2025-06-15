using UnityEngine;
using UnityEngine.Rendering.Universal;   // ⚠️ URP 2D Light

/// <summary>
/// InspectorLightManager2D
/// • 불빛이 서서히 켜졌다가 → ‘확’ 꺼졌다가를 반복
/// • PlayerJumpManager.FlickerIntense(0~1) 값으로 Spot Light 각도 제어
/// </summary>
[DisallowMultipleComponent]
public class InspectorLightManager2D : MonoBehaviour
{
    [Header("2D Light (URP)")]
    public Light2D fireflyLight2D;       // 2D 라이트 (비워두면 자동 탐색)

    [Header("Sprite (대체용)")]
    public SpriteRenderer sprite;        // 2D 스프라이트 알파 flicker

    [Header("Intensity")]
    public float minIntensity = 0f;
    public float maxIntensity = 1.3f;

    [Header("Pulse Timing (sec)")]
    public float riseTime = 1.0f;
    public float pauseTime = 0.25f;
    public float dropTime = 0.12f;      // ‘확’ 꺼짐

    [Header("Spot-Angle 링크")]
    [Tooltip("FlickerIntense = 1 ➜ 이 각도; Light2D 타입이 Spot일 때만 사용")]
    public float maxSpotAngle = 55f;

    /* ───────── 내부 ───────── */
    enum Phase { Rising, HoldBright, Dropping, HoldDark }
    Phase phase = Phase.Rising;
    float t = 0f;               // 0~1 진행

    public void setColor(FireflyBehaviourSmooth.CenterType type)
    {
        if(type == FireflyBehaviourSmooth.CenterType.main)
        {
            fireflyLight2D.color = new Color(1f, 0.72f, 0.53f, 1f);
        }
        else
        {
            fireflyLight2D.color = new Color(1f, 0.41f, 0f, 1f);
        }
    }

    void Reset()
    {
        // 에디터에서 컴포넌트 붙일 때 자동 연결
        fireflyLight2D = GetComponent<Light2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!fireflyLight2D) fireflyLight2D = GetComponent<Light2D>();
        if (!sprite) sprite = GetComponent<SpriteRenderer>();

        /* 1) PlayerJumpManager.FlickerIntense → Spot 각도 */
        fireflyLight2D.pointLightInnerAngle = playerJumpManager.now.FlickerIntense * maxSpotAngle;
        fireflyLight2D.pointLightOuterAngle = playerJumpManager.now.FlickerIntense * maxSpotAngle;

        /* 2) “켜짐-유지-꺼짐-유지” 루프 */
        switch (phase)
        {
            case Phase.Rising:
                t += Time.deltaTime / riseTime;
                ApplyIntensity(Mathf.Lerp(minIntensity, maxIntensity, t));
                if (t >= 1f) { phase = Phase.HoldBright; t = 0f; }
                break;

            case Phase.HoldBright:
                ApplyIntensity(maxIntensity);
                if ((t += Time.deltaTime) >= pauseTime) { phase = Phase.Dropping; t = 0f; }
                break;

            case Phase.Dropping:
                t += Time.deltaTime / dropTime;
                ApplyIntensity(Mathf.Lerp(maxIntensity, minIntensity, t));
                if (t >= 1f) { phase = Phase.HoldDark; t = 0f; }
                break;

            case Phase.HoldDark:
                ApplyIntensity(minIntensity);
                if ((t += Time.deltaTime) >= pauseTime) { phase = Phase.Rising; t = 0f; }
                break;
        }
    }

    /* ───────── 공통 intensity 적용 ───────── */
    void ApplyIntensity(float value)
    {
        if (fireflyLight2D) fireflyLight2D.intensity = value;

        if (sprite)
        {
            var c = sprite.color;
            c.a = Mathf.Clamp01(value / maxIntensity);  // 0~1 로 매핑
            sprite.color = c;
        }
    }
}
