using UnityEngine;

/// <summary>
/// 装备接口，所有装备类型都必须实现此接口
/// </summary>
public interface IEquipment
{
    string GetEquipmentName();
    Sprite GetEquipmentIcon();
    string GetEquipmentId();
    EquipmentType GetEquipmentType();
    void AddModifiers();
    void RemoveModifiers();
    void UseDynamicItemEffect(Transform target);
    void UsePassiveItemEffect(Transform target);
    bool CoolDownCounter();
    float GetCoolDownTime();
} 