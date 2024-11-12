using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    private void OnValidate()
    {
        // ͬ�� itemName �Ͷ�������
        itemName = this.name;
    }
}
