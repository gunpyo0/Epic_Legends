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
        // 나중에 더 아이템이 나올 수 있을 것 같아서 만들었음
    }

    public ItemType itemType;

    public string itemName;
    public Sprite icon;
    public bool canDuplicate;

}
