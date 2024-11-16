
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLooseItems;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;
        List<InventoryItem> currentEquipment = inventory.GetEquipmentList();

        foreach (InventoryItem item in currentEquipment)
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                DropItem(item.itemData);
                Debug.Log("随机抽到物体了");   
                //inventory.UnEquip(item.itemData as ItemData_Equipment);
            }
        }
    }
}
