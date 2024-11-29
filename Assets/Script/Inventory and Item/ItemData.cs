using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    
    public ItemType type;
    public string itemName;
    public Sprite icon;
    public string itemId;

    [Range(0, 100)]
    public float dropChance;
    protected void OnValidate()
    {
        // 同步 itemName 和对象名称
         itemName = this.name;
        //分配物品ID
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }
}
