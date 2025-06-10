using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Fragment,
        // ���߿� �� �������� ���� �� ���� �� ���Ƽ� �������
    }

    public ItemType itemType;

    public string itemName;
    public Sprite icon;


}
