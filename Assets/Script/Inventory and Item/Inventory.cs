using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;

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

    [SerializeField] private List<InventoryItem> StartEquipmentList;

    private UI_ItemSlot[] itemSlot;
    private UI_ItemSlot[] stashSlot;
    private UI_EquipmentSlot[] equipmentSlot;

    [Header("Equipment Counter")]

    private float lastTimeUsed;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ����ʵ���ڳ����л�ʱ��������
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

        //����Ƿ�����ͬ���͵���Ʒ
        foreach ( KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionaryList)
        {
            if(item.Key.equipmentType == newEquipment.equipmentType)//��item in equipmentDictionaryListһ��һ����newEquipment�Ա�
            {
                oldEquipment = item.Key;
            }
        }
        //�Ƴ�����Ʒ
        if (oldEquipment != null)
        {
            UnEquip(oldEquipment);
            AddItem(oldEquipment, 1);
        }
        //�����Ʒ
        equipmentList.Add(newItem);
        equipmentDictionaryList.Add(newEquipment, newItem);
        RemoveItem(newItem.itemData, 1);

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

    public void AddItem(ItemData _item, int _amountToAdd)//���ֿ������Ʒ ItemDataΪ��Ʒ���ͣ� amountToAddΪ��ӵ�����
    {
        if (_item.type == ItemType.Equipment)//�ж�����������Ʒ
            AddEquipment(_item, _amountToAdd);
        if (_item.type == ItemType.Material)
            AddMaterial(_item, _amountToAdd);

        UpdateSlotUI();

    }

    private void AddMaterial(ItemData _item, int _amountToAdd)
    {
        if (stashDictionaryList.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack(_amountToAdd);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            newItem.stackSize = _amountToAdd;

            stashItemsList.Add(newItem);
            stashDictionaryList.Add(_item, newItem);
        }
    }

    private void AddEquipment(ItemData _item, int _amountToAdd)
    {
        if (inventoryDictionaryList.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack(_amountToAdd);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            newItem.stackSize = _amountToAdd;

            inventoryItemsList.Add(newItem);
            inventoryDictionaryList.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item, int _amountToRemove)//����һ��ɾ_amountToRemove��
    {
        if (inventoryDictionaryList.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= _amountToRemove)
            {
                inventoryItemsList.Remove(value);
                inventoryDictionaryList.Remove(_item);
            }
            else
            {
                value.RemoveStack(_amountToRemove);
            }
        }

        if (stashDictionaryList.TryGetValue(_item, out InventoryItem stashvalue))
        {
            if (stashvalue.stackSize <= _amountToRemove)
            {
                stashItemsList.Remove(stashvalue);
                stashDictionaryList.Remove(_item);
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

        for (int i = 0; i < _MaterialNeeded.Count; i++)//��stashDictionaryList�м��� �����Ƿ��㹻 
        {
            if(stashDictionaryList.TryGetValue(_MaterialNeeded[i].itemData, out InventoryItem value))
            {
                if (value.stackSize >= _MaterialNeeded[i].stackSize)//�����㹻�򽫲��ϼ���ItemToRemove��
                {
                    ItemToRemove.Add(value);                        //��һһ��������һ��������
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

        AddItem(itemToCreate, 1);//��Ϊ�����ǰ�һ�´���һ�Σ�����1���ܻ��
        Debug.Log("itemDone"+ itemToCreate.name);   
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipmentList;

    public ItemData_Equipment GetEquipedEquipment(EquipmentType _typeOfEquipment)//��ȡ��װ����װ���б� 
    {
        ItemData_Equipment equipedItem = null;      //�Լ�д�ģ� ���P115��equipedItem�ĳ���List����������װ������ ɾ���ˣ��ܶ�ط���ʵ��ֻ��Ҫ����һ�� ����Ҫ�Ӷ�װ��ϵͳ�ٸ�

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionaryList)
        {
            if (item.Key.equipmentType == _typeOfEquipment)
            {
                equipedItem = item.Key;
            }
        }
        return equipedItem;
    }

    public void UseFlask()//���Flask��CD��CDת���˾���
    {
        ItemData_Equipment _flask = GetEquipedEquipment(EquipmentType.Flask);

        lastTimeUsed = -(_flask.FlaskCoolDown);

            bool CanUseFlask = Time.time > _flask.FlaskCoolDown + lastTimeUsed;

            if (CanUseFlask)
            {
                lastTimeUsed = Time.time;
                _flask.UseDynamicItemEffect(null);
        }
            else
                Debug.Log("FlaskOnCoolDown");

    }
}
