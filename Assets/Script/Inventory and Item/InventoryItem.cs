using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int stackSize;

    public InventoryItem(ItemData _newItemData)
    {
        itemData = _newItemData;
        AddStack(1);
    }

    public void AddStack(int _amountToAdd) => stackSize += _amountToAdd;
    public void RemoveStack(int _amountToRemove) => stackSize -= _amountToRemove;
}
