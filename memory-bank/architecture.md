## 项目结构总览

- **引擎与平台**: Unity（含 Addressables 配置、HybridCLR 热更新能力）
- **主要目录**
  - `Assets/`
    - `Script/`
      - `Inventory and Item/`：物品/装备/背包与道具效果
      - `stats/`：角色属性与战斗数值系统
      - `Save and Load/`：存档系统
      - `UI/`：界面（物品槽、装备槽、属性面板）
      - `HotUpdate/`：HybridCLR 热更新加载流程
      - 其它：`Player/`、`Enemy/`、`Input/` 等
    - `Data/`：ScriptableObject 数据资源
      - `Item/`：物品与装备数据
      - `Effect/`：道具/装备效果数据
    - `AddressableAssetsData/`：Addressables 配置
    - 其它：`Scenes/`、`Prefabs/`、`Graphics/` 等
  - `StreamingAssets/`：HybridCLR 运行时加载资源（AOT 元数据、热更 dll、ab）
  - `HybridCLRData/`：HybridCLR 生成与配置数据
  - `memory-bank/`：架构与知识库

---

## 核心模块与职责

### 1) 物品与装备（Inventory & Item）
- `ItemData` (SO)
  - 字段：`type`(Material/Equipment)、`itemName`、`icon`、`itemId`、`dropChance`
  - 生成：编辑器下 `OnValidate` 自动用 `AssetDatabase.AssetPathToGUID` 生成 `itemId`
- `ItemData_Equipment` (SO, 实现 `IEquipment`)
  - 关键字段：`equipmentType`、`dynamicEffects[]`、`passvieEffects[]`、各类数值（主/攻/防/魔）、`CoolDownTime`、`craftingMaterials`
  - 行为：`AddModifiers()`/`RemoveModifiers()` 直接操作 `PlayerStats` 的各类 `Stats`；`UseDynamicItemEffect()`/`UsePassiveItemEffect()` 遍历 `itemEffect`
- `itemEffect` (SO基类)
  - 接口：`UseEffect(Transform target)`（子类在 `Assets/Data/Effect/` 下）
- `InventoryItem`（序列化类）
  - `itemData` + `stackSize`，用于 UI 与存取
- `Inventory`（单例，ISaveManager）
  - 容器：`inventoryDictionary`（装备待穿）、`stashDictionary`（材料仓）、`equipmentDictionary`（已装备）
  - 操作：`AddItem`（Material → 仓库；Equipment → 背包）、`RemoveItem`、`Equip`/`UnEquip`
  - 加载：编辑器用 `AssetDatabase.FindAssets`；运行时 `Resources.LoadAll<ItemData>("Data/Item")`
  - UI：`UpdateSlotUI()` 同步 `UI_ItemSlot`、`UI_EquipmentSlot`、`UI_StatSlot`
  - 存档：实现 `LoadData`/`SaveData`（详见下文 Schema）

### 2) 数值与战斗（stats）
- `CharacterStats`
  - 定义 `Stats` 集合（主/攻/防/魔），异常状态（Ignite/Chill/Shock）及生效逻辑，伤害结算、暴击、护甲、抗性等
  - 提供 `GetStat(StatType)` 供 UI 查询
- `PlayerStats`（继承 `CharacterStats`）
  - 受击时（血量低于 10%）触发已装备 `Armor` 的 `passiveEffects`（带冷却）

### 3) UI
- `UI_ItemSlot`：背包物品格（点击装备 / Ctrl-点击丢弃1个、悬停展示）
- `UI_EquipmentSlot`：装备槽（按 `EquipmentType` 匹配，点击卸下）
- `UI_StatSlot`：从 `PlayerStats` 拉取并显示数值

### 4) 存档系统（Save and Load）
- 接口：`ISaveManager`（`LoadData` / `SaveData`）
- 数据类型：`GameData`
- 管理：`SaveManager`、`FileDataHandler`、`SerializableDictionary`

---

## 数据与资源流

- 资源组织
  - 道具与装备：`Assets/Data/Item/{Material,Equipment}/...`（SO）
  - 效果：`Assets/Data/Effect/*.asset`（SO，派生自 `itemEffect`）
- 加载路径
  - 编辑器：`Inventory.FillUpItemDataBase()` → `AssetDatabase.FindAssets`
  - 运行时：`Inventory.LoadItemDataFromResources()` → `Resources.LoadAll<ItemData>("Data/Item")`
  - 热更 DLL / AOT 元数据 / 预制体：`StreamingAssets`（由 `LoadDllzheng` 读取）
- 运行时装配
  - 玩家交互（UI）→ `Inventory.Equip/UnEquip` → 修改 `equipmentDictionary` → `ItemData_Equipment.AddModifiers/RemoveModifiers` → `UI_StatSlot` 更新
  - 战斗触发 → `PlayerStats` 检测条件 → 调用 `ItemData_Equipment.UsePassiveItemEffect`

---

## 存档 Schema（数据库/持久化）

类：`GameData`
- `currency: int`
- `inventory: SerializableDictionary<string, int>`
  - key: `itemId`（GUID）
  - value: 数量
- `equipmentId: List<string>`
  - 已装备物品的 `itemId`
- `chekcpoints: SerializableDictionary<string, bool>`
- `closestCheckPointID: string`

读写策略：
- 保存：`Inventory.SaveData()`
  - 遍历 `inventoryDictionary` 与 `stashDictionary` 写入 `inventory`
  - 遍历 `equipmentDictionary` 写入 `equipmentId`
- 读取：`Inventory.LoadData()`
  - 将 `GameData.inventory` 与 `equipmentId` 对应到本地 `itemDataBase` 中的 `ItemData` / `ItemData_Equipment`，填充 `loadedItems`、`loadedEquipment`，并在 `Start()` 的 `AddStartingItems()` 中恢复

兼容性：
- 依赖 `itemId`（GUID）映射到本地 `ItemData`。若远程热更新增物品，需保证运行时可获取其 `ItemData` 资源，否则会丢失映射。

---

## 热更新管线（当前）

- 框架：HybridCLR
  - 入口：`Assets/Script/HotUpdate/LoadDLLzheng.cs`
  - 启动：`Start()` → 下载 `StreamingAssets` 中的 `HotUpdate.dll.bytes`、AOT 元数据（`mscorlib.dll.bytes` 等）
  - 加载：`LoadMetadataForAOTAssemblies()` → `Assembly.Load`/编辑器下直接引用 → 反射调用 `Entry.Start()`
  - 资源：从 `StreamingAssets` 加载 AssetBundle（如 `prefabs`）并实例化，验证热更脚本挂载

- 与物品系统的关系：
  - 目前物品/装备/效果仍通过本地 `SO + Resources` 加载。若要“新增装备/效果热更”，应迁移到 Addressables 或 AssetBundle + 远程 Catalog/AB 分发，并在运行时注册/加载。

---

## 现有痛点与演进建议（装备与效果热更）

- 痛点
  - 装备效果由 `ItemData_Equipment` 直接操纵 `PlayerStats`（紧耦合，不利于数据驱动与热更）
  - 运行时加载使用 `Resources.LoadAll`，无法远程增量分发新 `SO`
  - 效果 `itemEffect` 缺少统一注册/标识，无法通过“数据ID → 执行器”动态路由

- 建议路径
  1. 资源分发：将 `ItemData`、`itemEffect`、图标/模型迁移到 Addressables，启用远程 Catalog（CDN），实现新增资源的在线加载
  2. 效果抽象：引入 `IEquipmentEffect`/`EffectRegistry`/`EffectPipeline`，用 `EffectSpec{effectId, params}` 描述效果；在 `Inventory.Equip/UnEquip` 与战斗触发点改为调用管线
  3. 存档兼容：继续使用 `itemId`；若效果需要维护状态，增加 `itemInstanceId → effectState` 的可选保存
  4. 过渡期：保留 `AddModifiers/RemoveModifiers` 作为“属性加成算子”的一个实现

---

## 模块间依赖关系（简图）

- UI（`UI_ItemSlot`/`UI_EquipmentSlot`/`UI_StatSlot`）
  → 交互驱动 `Inventory`
  → 读取 `InventoryItem`
  → 展示 `ItemData`、`PlayerStats`

- `Inventory`
  → 维护 `inventoryDictionary`、`stashDictionary`、`equipmentDictionary`
  → 装备/卸下调用 `ItemData_Equipment` 的效果或未来的 `EffectPipeline`
  → 存档通过 `ISaveManager` 与 `GameData` 交互
  → 加载通过 `Resources`（建议迁移 Addressables）

- `ItemData` / `ItemData_Equipment` / `itemEffect`
  → 数据资源（SO）；效果执行直接或通过管线操作 `PlayerStats`

- `PlayerStats`（数值）
  → 被装备效果修改；向 UI 暴露 `GetStat`

- 热更新（HybridCLR）
  → 负责 C# 逻辑热更与 AB 资源加载；与物品系统的资源/配置热更应打通

---

## 里程碑建议

- M1：将 `ItemData`、`itemEffect`、图标等迁移至 Addressables，建立远程 Catalog；为 `Inventory` 增加 Addressables 加载路径
- M2：定义 `EffectSpec`、`IEquipmentEffect`、`EffectRegistry`，实现“属性加成/治疗/护盾”等基础算子，改造 `Equip/UnEquip`
- M3：在 `PlayerStats` 的被动触发点接入 `EffectPipeline`；实现效果冷却、叠层、持续时间等通用机制
- M4：完善存档兼容与错误回退（未知 effectId 忽略并记录告警）
- M5：必要时对少量复杂效果引入 Lua/HybridCLR 可插拔扩展点
