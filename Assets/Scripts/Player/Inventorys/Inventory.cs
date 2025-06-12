using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemCombiner combiner;
    public List<Item> items = new List<Item>();

    public void AddItem(Item item)
    {
        if (items.Contains(item) && !item.canDuplicate) return;
        items.Add(item);
        combiner.TryCombine(this);
    }

    public bool HasItems(List<ItemRecipe.Ingredient> ingredients)
    {
        foreach (var ingredient in ingredients)
        {
            int count = items.Count(i => i == ingredient.item);
            if (count < ingredient.amount)
                return false;
        }
        return true;
    }

    public void RemoveItems(List<ItemRecipe.Ingredient> ingredients)
    {
        foreach (var ingredient in ingredients)
        {
            int removed = 0;
            for (int i = items.Count - 1; i >= 0 && removed < ingredient.amount; i--)
            {
                if (items[i] == ingredient.item)
                {
                    items.RemoveAt(i);
                    removed++;
                }
            }
        }
    }

    public void UseItem(Item item, GameObject user)
    {
        if (item is ConsumableItem consumable && consumable.effect != null)
        {
            float? duration = consumable.customDuration > 0f ? consumable.customDuration : null;
            consumable.effect.Apply(user, duration);
        }
    }

  

}

