// using UnityEngine;

// /// <summary>
// /// 热更新装备系统使用示例
// /// 这个脚本展示了如何在游戏中使用热更新装备系统
// /// </summary>
// public class HotUpdateUsageExample : MonoBehaviour
// {
//     [Header("测试按钮")]
//     [SerializeField] private bool testCreateFireWeapon = false;
//     [SerializeField] private bool testCreateIceArmor = false;
//     [SerializeField] private bool testAddEffects = false;

//     private void Start()
//     {
//         // 确保热更新管理器存在
//         if (HotUpdateEquipmentManager.Instance == null)
//         {
//             GameObject managerGO = new GameObject("HotUpdateEquipmentManager");
//             managerGO.AddComponent<HotUpdateEquipmentManager>();
//         }

//         // 注册热更新装备类型（通常在热更新加载时调用）
//         RegisterHotUpdateTypes();
//     }

//     private void Update()
//     {
//         // 测试创建火焰武器
//         if (testCreateFireWeapon)
//         {
//             testCreateFireWeapon = false;
//             CreateFireWeapon();
//         }

//         // 测试创建冰霜护甲
//         if (testCreateIceArmor)
//         {
//             testCreateIceArmor = false;
//             CreateIceArmor();
//         }

//         // 测试添加效果
//         if (testAddEffects)
//         {
//             testAddEffects = false;
//             AddEffectsToEquipment();
//         }
//     }

//     /// <summary>
//     /// 注册热更新类型
//     /// 这个方法通常在热更新程序集加载后调用
//     /// </summary>
//     private void RegisterHotUpdateTypes()
//     {
//         var manager = HotUpdateEquipmentManager.Instance;

//         // 注册装备类型
//         manager.RegisterEquipmentType("FireWeapon", typeof(HotUpdateFireWeapon));
//         manager.RegisterEquipmentType("IceArmor", typeof(HotUpdateIceArmor));

//         // 注册效果类型
//         manager.RegisterEffectType("PoisonEffect", typeof(HotUpdatePoisonEffect));
//         manager.RegisterEffectType("LightningEffect", typeof(HotUpdateLightningEffect));

//         Debug.Log("热更新装备类型注册完成");
//     }

//     /// <summary>
//     /// 创建火焰武器示例
//     /// </summary>
//     private void CreateFireWeapon()
//     {
//         var manager = HotUpdateEquipmentManager.Instance;
        
//         // 创建火焰武器实例
//         HotUpdateFireWeapon fireWeapon = manager.CreateEquipmentInstance("FireWeapon", "fire_weapon_001") as HotUpdateFireWeapon;
        
//         if (fireWeapon != null)
//         {
//             // 设置装备属性
//             fireWeapon.equipmentType = EquipmentType.Weapon;
//             fireWeapon.dealDamage = 50;
//             fireWeapon.fireDamage = 30;
//             fireWeapon.CoolDownTime = 2f;
            
//             // 设置图标（这里需要从资源加载）
//             // fireWeapon.icon = Resources.Load<Sprite>("Icons/fire_weapon");
            
//             // 添加到背包
//             Inventory.instance.AddItem(fireWeapon, 1);
            
//             Debug.Log($"成功创建火焰武器: {fireWeapon.itemName}");
//         }
//     }

//     /// <summary>
//     /// 创建冰霜护甲示例
//     /// </summary>
//     private void CreateIceArmor()
//     {
//         var manager = HotUpdateEquipmentManager.Instance;
        
//         // 创建冰霜护甲实例
//         HotUpdateIceArmor iceArmor = manager.CreateEquipmentInstance("IceArmor", "ice_armor_001") as HotUpdateIceArmor;
        
//         if (iceArmor != null)
//         {
//             // 设置装备属性
//             iceArmor.equipmentType = EquipmentType.Armor;
//             iceArmor.armor = 25;
//             iceArmor.iceDamage = 20;
//             iceArmor.CoolDownTime = 5f;
            
//             // 设置图标
//             // iceArmor.icon = Resources.Load<Sprite>("Icons/ice_armor");
            
//             // 添加到背包
//             Inventory.instance.AddItem(iceArmor, 1);
            
//             Debug.Log($"成功创建冰霜护甲: {iceArmor.itemName}");
//         }
//     }

//     /// <summary>
//     /// 为装备添加热更新效果示例
//     /// </summary>
//     private void AddEffectsToEquipment()
//     {
//         var manager = HotUpdateEquipmentManager.Instance;
        
//         // 获取已创建的装备
//         HotUpdateFireWeapon fireWeapon = manager.GetDynamicEquipment("fire_weapon_001") as HotUpdateFireWeapon;
        
//         if (fireWeapon != null)
//         {
//             // 添加毒药效果
//             manager.AddEffectToEquipment(fireWeapon, "PoisonEffect", true);
            
//             // 添加闪电效果
//             manager.AddEffectToEquipment(fireWeapon, "LightningEffect", false);
            
//             Debug.Log($"为火焰武器添加了热更新效果");
//         }
//     }

//     /// <summary>
//     /// 从配置文件加载热更新装备
//     /// 这个方法展示了如何从JSON配置文件动态创建装备
//     /// </summary>
//     public void LoadEquipmentFromConfig(string configJson)
//     {
//         // 这里可以解析JSON配置
//         // 示例配置格式：
//         /*
//         {
//             "equipmentType": "FireWeapon",
//             "equipmentId": "fire_weapon_002",
//             "equipmentName": "烈焰之剑",
//             "stats": {
//                 "dealDamage": 60,
//                 "fireDamage": 40
//             },
//             "effects": [
//                 {
//                     "effectType": "PoisonEffect",
//                     "isDynamic": true
//                 }
//             ]
//         }
//         */
        
//         // 解析配置并创建装备的逻辑
//         Debug.Log("从配置文件加载装备的功能待实现");
//     }

//     /// <summary>
//     /// 热更新装备的完整使用流程示例
//     /// </summary>
//     public void CompleteEquipmentUsageExample()
//     {
//         var manager = HotUpdateEquipmentManager.Instance;
        
//         // 1. 创建装备
//         HotUpdateFireWeapon weapon = manager.CreateEquipmentInstance("FireWeapon", "test_weapon") as HotUpdateFireWeapon;
        
//         if (weapon != null)
//         {
//             // 2. 配置装备属性
//             weapon.equipmentType = EquipmentType.Weapon;
//             weapon.dealDamage = 45;
//             weapon.fireDamage = 25;
//             weapon.CoolDownTime = 3f;
            
//             // 3. 添加效果
//             manager.AddEffectToEquipment(weapon, "PoisonEffect", true);
            
//             // 4. 添加到背包
//             Inventory.instance.AddItem(weapon, 1);
            
//             // 5. 装备
//             Inventory.instance.Equip(weapon);
            
//             // 6. 调用装备的特殊方法
//             weapon.OnEquip();
            
//             Debug.Log("热更新装备使用流程完成");
//         }
//     }
// } 