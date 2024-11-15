using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftUI : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdataSlot(item);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment _trunedItem = item.itemData as ItemData_Equipment;

        if(Inventory.instance.CanCraft(_trunedItem, _trunedItem.craftingMaterials))
        {
            Debug.Log("CanReturn");
            return;
        }


    }
}
