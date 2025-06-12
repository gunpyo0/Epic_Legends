using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase
{
    public static Item GetItemByName(string itemName)
    {
        return Resources.Load<Item>($"Items/{itemName}");
    }
}
