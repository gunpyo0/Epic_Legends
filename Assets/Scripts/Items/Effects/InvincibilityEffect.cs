using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/InvincibilityEffect")]
public class InvincibilityEffect : ItemEffect
{
    public float defaultDuration = 30f;

    public override void Apply(GameObject target, float? overrideDuration = null)
    {
        var player = target.GetComponent<PlayerHp>();
        if (player != null)
        {
            float actualDuration = overrideDuration ?? defaultDuration;
            player.StartCoroutine(InvincibilityCoroutine(player, actualDuration));
        }
    }

    private IEnumerator InvincibilityCoroutine(PlayerHp player, float duration)
    {
        player.IsInvinc = true;
        yield return new WaitForSeconds(duration);
        player.IsInvinc = false;
    }
}

