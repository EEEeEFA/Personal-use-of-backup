using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 热更新装备管理器，负责动态加载和管理热更新装备
/// </summary>
public class HotUpdateEquipmentManager : MonoBehaviour
{
    public static HotUpdateEquipmentManager Instance { get; private set; }

    // 存储已注册的热更新装备类型
    private Dictionary<string, Type> registeredEquipmentTypes = new Dictionary<string, Type>();
    
    // 存储已注册的热更新效果类型
    private Dictionary<string, Type> registeredEffectTypes = new Dictionary<string, Type>();

    // 存储动态创建的装备实例
    private Dictionary<string, HotUpdateEquipmentBase> dynamicEquipmentInstances = new Dictionary<string, HotUpdateEquipmentBase>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 注册热更新装备类型
    /// </summary>
    /// <param name="equipmentName">装备名称</param>
    /// <param name="equipmentType">装备类型</param>
    public void RegisterEquipmentType(string equipmentName, Type equipmentType)
    {
        if (equipmentType.IsSubclassOf(typeof(HotUpdateEquipmentBase)))
        {
            registeredEquipmentTypes[equipmentName] = equipmentType;
            Debug.Log($"注册热更新装备类型: {equipmentName} -> {equipmentType.Name}");
        }
        else
        {
            Debug.LogError($"注册装备类型失败: {equipmentType.Name} 不是 HotUpdateEquipmentBase 的子类");
        }
    }

    /// <summary>
    /// 注册热更新效果类型
    /// </summary>
    /// <param name="effectName">效果名称</param>
    /// <param name="effectType">效果类型</param>
    public void RegisterEffectType(string effectName, Type effectType)
    {
        if (effectType.IsSubclassOf(typeof(itemEffect)))
        {
            registeredEffectTypes[effectName] = effectType;
            Debug.Log($"注册热更新效果类型: {effectName} -> {effectType.Name}");
        }
        else
        {
            Debug.LogError($"注册效果类型失败: {effectType.Name} 不是 itemEffect 的子类");
        }
    }

    /// <summary>
    /// 创建热更新装备实例
    /// </summary>
    /// <param name="equipmentName">装备名称</param>
    /// <param name="equipmentId">装备ID</param>
    /// <returns>装备实例</returns>
    public HotUpdateEquipmentBase CreateEquipmentInstance(string equipmentName, string equipmentId)
    {
        if (!registeredEquipmentTypes.ContainsKey(equipmentName))
        {
            Debug.LogError($"未找到注册的装备类型: {equipmentName}");
            return null;
        }

        try
        {
            Type equipmentType = registeredEquipmentTypes[equipmentName];
            HotUpdateEquipmentBase equipment = ScriptableObject.CreateInstance(equipmentType) as HotUpdateEquipmentBase;
            
            if (equipment != null)
            {
                equipment.itemId = equipmentId;
                equipment.itemName = equipmentName;
                equipment.type = ItemType.Equipment;
                
                // 存储实例
                dynamicEquipmentInstances[equipmentId] = equipment;
                
                Debug.Log($"成功创建热更新装备实例: {equipmentName} (ID: {equipmentId})");
                return equipment;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"创建装备实例失败: {e.Message}");
        }

        return null;
    }

    /// <summary>
    /// 创建热更新效果实例
    /// </summary>
    /// <param name="effectName">效果名称</param>
    /// <returns>效果实例</returns>
    public itemEffect CreateEffectInstance(string effectName)
    {
        if (!registeredEffectTypes.ContainsKey(effectName))
        {
            Debug.LogError($"未找到注册的效果类型: {effectName}");
            return null;
        }

        try
        {
            Type effectType = registeredEffectTypes[effectName];
            itemEffect effect = ScriptableObject.CreateInstance(effectType) as itemEffect;
            
            if (effect != null)
            {
                Debug.Log($"成功创建热更新效果实例: {effectName}");
                return effect;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"创建效果实例失败: {e.Message}");
        }

        return null;
    }

    /// <summary>
    /// 为装备添加热更新效果
    /// </summary>
    /// <param name="equipment">装备实例</param>
    /// <param name="effectName">效果名称</param>
    /// <param name="isDynamic">是否为动态效果</param>
    public void AddEffectToEquipment(HotUpdateEquipmentBase equipment, string effectName, bool isDynamic = true)
    {
        itemEffect effect = CreateEffectInstance(effectName);
        if (effect != null)
        {
            if (isDynamic)
            {
                AddToArray(ref equipment.dynamicEffects, effect);
            }
            else
            {
                AddToArray(ref equipment.passiveEffects, effect);
            }
        }
    }

    /// <summary>
    /// 获取动态创建的装备实例
    /// </summary>
    /// <param name="equipmentId">装备ID</param>
    /// <returns>装备实例</returns>
    public HotUpdateEquipmentBase GetDynamicEquipment(string equipmentId)
    {
        if (dynamicEquipmentInstances.ContainsKey(equipmentId))
        {
            return dynamicEquipmentInstances[equipmentId];
        }
        return null;
    }

    /// <summary>
    /// 检查装备类型是否已注册
    /// </summary>
    /// <param name="equipmentName">装备名称</param>
    /// <returns>是否已注册</returns>
    public bool IsEquipmentTypeRegistered(string equipmentName)
    {
        return registeredEquipmentTypes.ContainsKey(equipmentName);
    }

    /// <summary>
    /// 检查效果类型是否已注册
    /// </summary>
    /// <param name="effectName">效果名称</param>
    /// <returns>是否已注册</returns>
    public bool IsEffectTypeRegistered(string effectName)
    {
        return registeredEffectTypes.ContainsKey(effectName);
    }

    /// <summary>
    /// 获取所有已注册的装备类型
    /// </summary>
    /// <returns>装备类型名称列表</returns>
    public List<string> GetRegisteredEquipmentTypes()
    {
        return new List<string>(registeredEquipmentTypes.Keys);
    }

    /// <summary>
    /// 获取所有已注册的效果类型
    /// </summary>
    /// <returns>效果类型名称列表</returns>
    public List<string> GetRegisteredEffectTypes()
    {
        return new List<string>(registeredEffectTypes.Keys);
    }

    /// <summary>
    /// 辅助方法：向数组添加元素
    /// </summary>
    private void AddToArray<T>(ref T[] array, T newElement)
    {
        if (array == null)
        {
            array = new T[1];
        }
        else
        {
            Array.Resize(ref array, array.Length + 1);
        }
        array[array.Length - 1] = newElement;
    }

    /// <summary>
    /// 清理所有动态创建的实例
    /// </summary>
    public void ClearDynamicInstances()
    {
        foreach (var equipment in dynamicEquipmentInstances.Values)
        {
            if (equipment != null)
            {
                DestroyImmediate(equipment);
            }
        }
        dynamicEquipmentInstances.Clear();
    }
} 