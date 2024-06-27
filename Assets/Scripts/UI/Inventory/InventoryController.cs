using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;
using System.Linq;

public class InventoryController : IInitializable
{
    public event Action OnInventoryCleared;
    public event Action<int, IItem> OnItemAdded;
    public event Action<int, IItem> OnItemRemoved;

    public IItem[] Items { get; private set; }

    public Transform Store3DItems { get; private set; }

    private IngredientType[] _recipeIngrTypes = new IngredientType[2];
    private CraftRecipe _recipe;

    [Inject]
    public void Construct(CraftRecipe recipe)
    {
        _recipe = recipe;
    }

    public void Initialize()
    {
        Items = new IItem[10];
        Store3DItems = GameObject.Find("Store3DItems").transform;

        _recipeIngrTypes[0] = _recipe.ingredients[0].IngredientType;
        _recipeIngrTypes[1] = _recipe.ingredients[1].IngredientType;
    }

    public void AddToInventory(IItem item)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == null)
            {
                Items[i] = item;
                OnItemAdded?.Invoke(i,item);
                break;
            }              
        }       
    }

    public void DeleteFromInventory(IItem item)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == item)
            {
                Items[i] = null;
                OnItemRemoved?.Invoke(i, item);

                foreach (Transform child in Store3DItems.transform)
                {
                    if (child.TryGetComponent(out IItem item1) && item1 == item)
                        UnityEngine.Object.Destroy(child.gameObject);
                }

                break;
            }
        }
    }

    public void ClearInventory()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i] = null;               
        }

        OnInventoryCleared?.Invoke();
    }

    public void Consume(InventorySlot[] Slots)
    {
        foreach (InventorySlot slot in Slots)
        {
            if (slot.IsSelected)
            { 
                if (slot.currentItem is IConsumable consItem)
                {
                    consItem.Consume();
                    DeleteFromInventory(slot.currentItem);                 
                }
                else
                    Debug.Log(slot.currentItem + " is not consumable!");
            }
        }
    }

    public void Craft(InventorySlot[] Slots)
    {
        IItem[] craftMaterials = new IItem[2];
        IngredientType[] craftMaterialTypes = new IngredientType[2];
        int i = 0;
        
        foreach (InventorySlot slot in Slots)
        {
            if (slot.IsSelected)
            {
                if (slot.currentItem is ICraftable craftItem)
                {
                    craftMaterials[i] = slot.currentItem;
                    craftMaterialTypes[i] = craftItem.IngredientType;
                    i++;
                }
                else
                    Debug.Log(slot.currentItem + " is not craftable!");
            }

            if (i == 2)
                break;
        }
        
        if (i < 2)
        {
            Debug.Log("There are not enough materials(" + i + ") for the crafting!");
            return;
        }

        IEnumerable<IngredientType> result = craftMaterialTypes.Except(_recipeIngrTypes);
        if (result.Count() == 0)
        {
            DeleteFromInventory(craftMaterials[0]);
            DeleteFromInventory(craftMaterials[1]);
            AddToInventory(_recipe.product);
        }
    }

    
}
