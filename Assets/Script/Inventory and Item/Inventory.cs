using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventoryItemsList;
    public Dictionary<ItemData, InventoryItem> inventoryDictionaryList;

    public List<InventoryItem> stashItemsList;
    public Dictionary<ItemData, InventoryItem> stashDictionaryList;

    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;


    private UI_ItemSlot[] itemSlot;
    private UI_ItemSlot[] stashSlot;
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

        itemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
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
            if (value.stackSize <= 1)
            {
                stashItemsList.Remove(value);
                stashDictionaryList.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
        UpdateSlotUI();
    }
    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItemsList.Count; i++)
            itemSlot[i].UpdataSlot(inventoryItemsList[i]);

        for (int i = 0; i < stashItemsList.Count; i++)
            stashSlot[i].UpdataSlot(stashItemsList[i]);
    }
}
