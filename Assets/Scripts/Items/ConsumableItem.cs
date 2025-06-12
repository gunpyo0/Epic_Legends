using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ConsumableItem")]
public class ConsumableItem : Item
{
    public ItemEffect effect;

    [Tooltip("�� �������� �ο��� ���ӽð�. 0�̸� �⺻�� ���")]
    public float customDuration = 0f;

    private void OnEnable()
    {
        itemType = ItemType.ConsumableItem;
    }
}
