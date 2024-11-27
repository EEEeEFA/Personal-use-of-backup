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
        if (item == null) 
            return;

        itemNameText.text = item.name;
        itemTypeText.text = item.equipmentType.ToString();
        //itemDescription.text = item.GetDescription();

        gameObject.SetActive(true);

    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
