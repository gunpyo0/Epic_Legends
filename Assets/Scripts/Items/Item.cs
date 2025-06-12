using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Fragment,
        ConsumableItem,
        // ���߿� �� �������� ���� �� ���� �� ���Ƽ� �������
    }

    public ItemType itemType;

    public string itemName;
    public Sprite icon;
    public bool canDuplicate;

}
