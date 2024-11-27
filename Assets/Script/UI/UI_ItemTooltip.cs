using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText; // 名称显示文本
    [SerializeField] private TextMeshProUGUI itemTypeText; // 类型显示文本
    [SerializeField] private TextMeshProUGUI itemDescription;// 描述显示文本

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item = null) 
            return;
        Debug.Log(item.itemName);
        //itemNameText.text = item.name;
        //itemTypeText.text = item.equipmentType.ToString();
        //itemDescription.text = item.GetDescription();

        //修改字体大小，防止字体过长
        //if(itemDescription.text.Length > 12)
        //    itemNameText.fontSize = itemNameText.fontSize * .7f;
        //else
        //    itemNameText.fontSize = defaultFontSize;

        gameObject.SetActive(true);

    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
