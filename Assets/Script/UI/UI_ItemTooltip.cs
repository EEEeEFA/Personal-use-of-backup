using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText; // ������ʾ�ı�
    [SerializeField] private TextMeshProUGUI itemTypeText; // ������ʾ�ı�
    [SerializeField] private TextMeshProUGUI itemDescription;// ������ʾ�ı�

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item = null) 
            return;
        Debug.Log(item.itemName);
        //itemNameText.text = item.name;
        //itemTypeText.text = item.equipmentType.ToString();
        //itemDescription.text = item.GetDescription();

        //�޸������С����ֹ�������
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
