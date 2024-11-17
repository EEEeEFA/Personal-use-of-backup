
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLooseItems;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList();

        List<InventoryItem> ItemToRemove = new List<InventoryItem>();

        foreach (InventoryItem item in currentEquipment)
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                DropItem(item.itemData);
                Debug.Log("随机抽到物体了");
                ItemToRemove.Add(item);

            }
        }
        for (int i = 0; i < ItemToRemove.Count; i++)
        {
            inventory.UnEquip(ItemToRemove[i].itemData as ItemData_Equipment);
        }

    }
}
