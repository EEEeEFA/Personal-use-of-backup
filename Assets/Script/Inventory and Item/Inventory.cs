using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventoryItemsList;
    public Dictionary<ItemData, InventoryItem> inventoryDictionaryList;

    public List<InventoryItem> stashItemsList;
    public Dictionary<ItemData, InventoryItem> stashDictionaryList;

    public List<InventoryItem> equipmentList;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionaryList;

    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;


    private UI_ItemSlot[] itemSlot;
    private UI_ItemSlot[] stashSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 保持实例在场景切换时不被销毁
        }
    }

    private void Start()
    {
        inventoryItemsList = new List<InventoryItem>();
        inventoryDictionaryList = new Dictionary<ItemData, InventoryItem>();

        stashItemsList = new List<InventoryItem>();
        stashDictionaryList = new Dictionary<ItemData, InventoryItem>();

        equipmentList = new List<InventoryItem>();
        equipmentDictionaryList = new Dictionary<ItemData_Equipment, InventoryItem>();

        itemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
    }

    public void Equip(ItemData _item)
    {

        ItemData_Equipment newEquipment = _item as ItemData_Equipment;

        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        //检测是否有相同物品
        foreach ( KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionaryList)
        {
            if(item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }
        //移除旧物品
        if (oldEquipment != null)
        {
            UnEquip(oldEquipment);
            AddItem(oldEquipment);
        }
        //添加物品
        equipmentList.Add(newItem);
        equipmentDictionaryList.Add(newEquipment, newItem);
        RemoveItem(newItem.itemData);

        newEquipment.AddModifiers();

        UpdateSlotUI();

    }

    public void UnEquip(ItemData_Equipment oldEquipment)
    {
        if (equipmentDictionaryList.TryGetValue(oldEquipment, out InventoryItem value))
        {
            equipmentList.Remove(value);
            equipmentDictionaryList.Remove(oldEquipment);
            oldEquipment.RemoveModifiers();
            
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.type == ItemType.Equipment)
            AddEquipment(_item);
        if (_item.type == ItemType.Material)
            AddMaterial(_item);

        UpdateSlotUI();

    }

    private void AddMaterial(ItemData _item)
    {
        if (stashDictionaryList.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stashItemsList.Add(newItem);
            stashDictionaryList.Add(_item, newItem);
        }
    }

    private void AddEquipment(ItemData _item)
    {
        if (inventoryDictionaryList.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItemsList.Add(newItem);
            inventoryDictionaryList.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionaryList.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItemsList.Remove(value);
                inventoryDictionaryList.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }

        if (stashDictionaryList.TryGetValue(_item, out InventoryItem stashvalue))
        {
            if (stashvalue.stackSize <= 1)
            {
                stashItemsList.Remove(value);
                stashDictionaryList.Remove(_item);
            }
            else
            {
                stashvalue.RemoveStack();
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

        for(int i = 0;i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionaryList)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdataSlot(item.Value);
                }
            }
        }
    }

    public bool CanCraft(ItemData_Equipment itemToCreate  , List<InventoryItem> _MaterialNeeded )
    {
        List<InventoryItem> ItemToRemove = new List<InventoryItem>();

        for (int i = 0; i < _MaterialNeeded.Count; i++)
        {
            if(stashDictionaryList.TryGetValue(_MaterialNeeded[i].itemData, out InventoryItem value))
            {
                if (value.stackSize >= _MaterialNeeded[i].stackSize)
                {
                    ItemToRemove.Add(value);
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
            RemoveItem(ItemToRemove[i].itemData); 
        }

        AddItem(itemToCreate);
        Debug.Log("itemDone"+ itemToCreate.name);
        return true;
    }

}
