using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCombiner : MonoBehaviour
{
    [SerializeField] private List<ItemRecipe> recipes;

    public bool TryCombine(Inventory inventory)
    {
        foreach (var recipe in recipes)
        {
            if (inventory.HasItems(recipe.ingredients))
            {
                inventory.RemoveItems(recipe.ingredients);
                inventory.AddItem(recipe.resultItem);
                Debug.Log($"조합 성공: {recipe.resultItem.name}");
                return true;
            }
        }
        return false;
    }
}
