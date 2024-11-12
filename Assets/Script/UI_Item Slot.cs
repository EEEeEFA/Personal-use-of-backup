using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class UI_ItemSlot : MonoBehaviour
{
    public InventoryItem item;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public void UpdataSlot(InventoryItem _newItem)
    {   
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
}
