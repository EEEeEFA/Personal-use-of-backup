using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    public InventoryItem item;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Inventory inventory = Inventory.instance;

        if (Input.GetKey(KeyCode.LeftControl))//按住leftControl，点左键，点一次扣一个
        {
            inventory.RemoveItem(item.itemData, 1);
            return; //不加这个return会把下面的装备也触发
        }

        if(item.itemData.type == ItemType.Equipment)
            inventory.Equip(item.itemData);
    }

    public void UpdataSlot(InventoryItem _newItem)
    {   
        itemImage.color = Color.white;

        item = _newItem;   

        if(item != null)
        {
            itemImage.sprite = item.itemData.icon; //图标

            if (item.stackSize > 1)//数量
                itemText.text = item.stackSize.ToString();
            else
                itemText.text = "";
        }
    }
    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }
}
