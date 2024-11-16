using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{ 
    public EquipmentType slotType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        Inventory.instance.UnEquip(item.itemData as ItemData_Equipment);
        Inventory.instance.AddItem(item.itemData as ItemData_Equipment, 1);
        CleanUpSlot();
    }

}
