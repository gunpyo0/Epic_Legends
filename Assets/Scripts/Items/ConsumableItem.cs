using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ConsumableItem")]
public class ConsumableItem : Item
{
    public ItemEffect effect;

    [Tooltip("이 아이템이 부여할 지속시간. 0이면 기본값 사용")]
    public float customDuration = 0f;

    private void OnEnable()
    {
        itemType = ItemType.ConsumableItem;
    }
}
