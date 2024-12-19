using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class Inventory : MonoBehaviour,ISaveManager
{
    public static Inventory instance;

    public List<InventoryItem> inventoryItemsList;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stashItemsList;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> equipmentList;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    [SerializeField] private List<InventoryItem> StartEquipmentList;


    private UI_ItemSlot[] itemSlot;
    private UI_ItemSlot[] stashSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("Equipment Counter")]
    private float lastTimeUsedFlask; 
    private float lastTimeUsedArmor;

    [Header("Data Base")]
    private string[] assetNames;
    private List<InventoryItem> loadedItems = new List<InventoryItem>();
    private List<ItemData_Equipment> loadedEquipment = new List<ItemData_Equipment>();

    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
//      DontDestroyOnLoad(gameObject); // 保持实例在场景切换时不被销毁
        }
    }

    private void Start()
    {
        inventoryItemsList = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stashItemsList = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipmentList = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        itemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();//获取初始装备

    }

    private void AddStartingItems()
    {
        foreach(ItemData_Equipment loadedEquipment in loadedEquipment)
        {
            Equip(loadedEquipment);
        }
        
        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                AddItem(item.itemData, item.stackSize);
            }

            return;
        }

        for (int i = 0; i < StartEquipmentList.Count; i++)
        {
            AddItem(StartEquipmentList[i].itemData, StartEquipmentList[i].stackSize);
        }
    }

    public void Equip(ItemData _item)
    {

        ItemData_Equipment newEquipment = _item as ItemData_Equipment;

        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        //检测是否有相同类型的物品
        foreach ( KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if(item.Key.equipmentType == newEquipment.equipmentType)//把item in equipmentDictionaryList一个一个和newEquipment对比
            {
                oldEquipment = item.Key;
            }
        }
        //移除旧物品
        if (oldEquipment != null)
        {
            UnEquip(oldEquipment);
            AddItem(oldEquipment, 1);
        }
        //添加物品
        equipmentList.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        RemoveItem(newItem.itemData, 1);

        newEquipment.AddModifiers();

        UpdateSlotUI();

    }

    public void UnEquip(ItemData_Equipment oldEquipment)
    {
        if (equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem value))
        {
            equipmentList.Remove(value);
            equipmentDictionary.Remove(oldEquipment);
            oldEquipment.RemoveModifiers();
            
        }

        UpdateSlotUI();
    }

    public void AddItem(ItemData _item, int _amountToAdd)//往仓库添加物品 ItemData为物品类型， amountToAdd为添加的数量
    {
        if (_item.type == ItemType.Material)
        {
            AddMaterial(_item, _amountToAdd);
        }
        if (_item.type == ItemType.Equipment && CanAddItem())//判断属于哪种物品
        {
            AddEquipment(_item, _amountToAdd);
        }

        UpdateSlotUI();

    }

    private void AddMaterial(ItemData _item, int _amountToAdd)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack(_amountToAdd);//AddStack是InventoryItem中自己创建的增加数量的函数
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            newItem.stackSize = _amountToAdd;

            stashItemsList.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddEquipment(ItemData _item, int _amountToAdd)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack(_amountToAdd);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            newItem.stackSize = _amountToAdd;

            inventoryItemsList.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public bool CanAddItem()
    {
        if(itemSlot.Length <= inventoryItemsList.Count)
            return false;
        else
            return true;
    }

    public void RemoveItem(ItemData _item, int _amountToRemove)//调用一次删_amountToRemove个
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= _amountToRemove)
            {
                inventoryItemsList.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack(_amountToRemove);
            }
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashvalue))
        {
            if (stashvalue.stackSize <= _amountToRemove)
            {
                stashItemsList.Remove(stashvalue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashvalue.RemoveStack(_amountToRemove);
            }
        }
        UpdateSlotUI();
    }
    private void UpdateSlotUI()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashSlot.Length; i++)
        {
            stashSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventoryItemsList.Count; i++)
        {
            itemSlot[i].UpdataSlot(inventoryItemsList[i]);
        }

        for (int i = 0; i < stashItemsList.Count; i++)
        {
            stashSlot[i].UpdataSlot(stashItemsList[i]);
        }

        for(int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdataSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }

    }

    public bool CanCraft(ItemData_Equipment itemToCreate  , List<InventoryItem> _MaterialNeeded )
    {
        List<InventoryItem> ItemToRemove = new List<InventoryItem>();

        for (int i = 0; i < _MaterialNeeded.Count; i++)//在stashDictionaryList中检索 材料是否足够 
        {
            if(stashDictionary.TryGetValue(_MaterialNeeded[i].itemData, out InventoryItem value))
            {
                if (value.stackSize >= _MaterialNeeded[i].stackSize)//材料足够则将材料加入ItemToRemove中
                {
                    ItemToRemove.Add(value);                        //万一一个够了另一个不够？
                }
                else
                {
                    Debug.Log("Not enough Material");
                    return false;
                }
            }
        }

        for(int i = 0; i < ItemToRemove.Count ; i++)
        {
            RemoveItem(ItemToRemove[i].itemData, _MaterialNeeded[i].stackSize);
            Debug.Log("item:" + ItemToRemove[i].itemData + "stackSize:"+ _MaterialNeeded[i].stackSize);
        }

        AddItem(itemToCreate, 1);//因为这里是按一下触发一次，后续1可能会变
        Debug.Log("itemDone"+ itemToCreate.name);   
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipmentList;

    public ItemData_Equipment GetEquipedEquipment(EquipmentType _typeOfEquipment)//获取被装备的装备
    {
        ItemData_Equipment equipedItem = null;      //自己写的： 相比P115把equipedItem改成了List方便后续多个装备触发 删掉了，很多地方其实都只需要调用一个 后续要加多装备系统再改

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _typeOfEquipment)
            {
                equipedItem = item.Key;
            }
        }
        return equipedItem;
    }

    public void LoadData(GameData _data)//加载数据
    {
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach(string loadedEquipmentId in _data.equipmentId)
        {
            foreach(var item in GetItemDataBase())
            {
                if(item != null && item.itemId == loadedEquipmentId)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach(KeyValuePair<ItemData_Equipment, InventoryItem>pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }
    }

    private List<ItemData> GetItemDataBase()//获取物品数据库
    {
        List<ItemData> itemDatabase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Item" });

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDatabase.Add(itemData);
        }

        return itemDatabase;

    }

    //public void UseFlask()//检测Flask的CD，CD转好了就用
    //{
    //    ItemData_Equipment _flask = GetEquipedEquipment(EquipmentType.Flask);

    //    lastTimeUsedFlask = -Mathf.Infinity;//防止游戏一开始用不了

    //    bool CanUseFlask = Time.time > _flask.FlaskCoolDown + lastTimeUsedFlask;

    //        if (CanUseFlask)
    //        {
    //            lastTimeUsedFlask = Time.time;
    //            _flask.UseDynamicItemEffect(null);
    //    }
    //        else
    //            Debug.Log("FlaskOnCoolDown");

    //}
    //public bool CanUseArmor()
    //{
    //    ItemData_Equipment _Armor = GetEquipedEquipment(EquipmentType.Armor);

    //    lastTimeUsedArmor = -Mathf.Infinity;

    //    if (Time.time > _Armor.ArmorCoolDown + lastTimeUsedArmor)
    //    {
    //        lastTimeUsedArmor = Time.time;
    //        return true;
    //    }
    //    else return false;
    //}
}
