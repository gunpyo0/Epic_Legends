using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<GameObject> inventory;

    public List<GameObject> GetInventory => inventory;

    public void AddToInventory(GameObject item)
    {
        if (inventory.Contains(item)) return;
        inventory.Add(item);
    }
}
