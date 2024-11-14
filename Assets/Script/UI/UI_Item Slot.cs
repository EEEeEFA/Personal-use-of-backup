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
        if(item.itemData.type == ItemType.Equipment)
        Inventory.instance.Equip(item.itemData);
    }

    public void UpdataSlot(InventoryItem _newItem)
    {   
        itemImage.color = Color.white;

        item = _newItem;   

        if(item != null)
        {
            itemImage.sprite = item.itemData.icon; //Í¼±ê

            if (item.stackSize > 1)//ÊýÁ¿
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
