using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemRecipes")]
public class ItemRecipe : ScriptableObject
{
    [System.Serializable]
    public struct Ingredient
    {
        public Item item;
        public int amount;
    }

    public List<Ingredient> ingredients;
    public Item resultItem;
}
