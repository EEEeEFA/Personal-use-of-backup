## 实施手册（迭代执行计划）

### 目标
- 将“`ItemData_Equipment` 直接操纵 `PlayerStats`”的紧耦合逻辑迁移到 `Inventory`，形成集中、可替换的效果应用层（为后续数据驱动与热更新铺路）。
- 建立可在线新增装备与效果的热更新能力（AssetBundle Browser 构建 AB + 远程分发 + 运行时注册/加载）。

### 范围
- 不改变现有数值与UI表现结果的前提下完成解耦。
- 逐步替换 `Resources` 加载为 AssetBundle 远程加载；保留本地回退。

---

## 迭代0：安全网与基线
- 建立回归用例
  - 记录典型装备（各 `EquipmentType`）在装备/卸下前后的 `PlayerStats` 数值快照
  - 战斗基础链路：普通攻击、暴击、护甲、生效的三系元素（Ignite/Chill/Shock）
  - 被动触发链路：`PlayerStats` 10% 血量触发 `Armor` 的 `passiveEffects`
- 日志探针
  - 关键节点打点：`Inventory.Equip/UnEquip`、`PlayerStats.TakeDamage/DoMagicDamage`、效果应用与移除
- 风险与回滚
  - 保留 `ItemData_Equipment.Add/RemoveModifiers`，在过渡期作为适配节点

验收标准
- 所有快照对比一致；关键路径无异常日志

---

## 迭代1：将 `PlayerStats` 操作迁移到 `Inventory`

设计要点
- 新增效果应用服务层：`Inventory` 内集中处理“装备→角色属性”的修改，避免 `ItemData_Equipment` 直接依赖 `PlayerStats`。
- 将现有 `AddModifiers/RemoveModifiers` 的字段映射迁移为 `Inventory` 内部方法，形成单一入口。

实施步骤
1) 在 `Inventory` 新增内部方法
   - `ApplyEquipmentStatModifiers(ItemData_Equipment equipment)`
   - `RemoveEquipmentStatModifiers(ItemData_Equipment equipment)`
   - 两者从 `ItemData_Equipment.Add/RemoveModifiers` 复制映射逻辑，并通过 `PlayerManager.instance.player.GetComponent<PlayerStats>()` 应用到 `Stats`
2) 修改 `Inventory.Equip`
   - 替换 `newEquipment.AddModifiers()` 为 `ApplyEquipmentStatModifiers(newEquipment)`
3) 修改 `Inventory.UnEquip`
   - 替换 `oldEquipment.RemoveModifiers()` 为 `RemoveEquipmentStatModifiers(oldEquipment)`
4) 兼容：保留 `ItemData_Equipment.Add/RemoveModifiers`（内部不再调用或改为委派到 `Inventory`），设为 Deprecated 注释
5) 回归测试：装备/卸下后属性变化与旧逻辑一致；`UI_StatSlot` 正常刷新

验收标准
- `ItemData_Equipment` 不再直接访问 `PlayerStats`
- 装备/卸下功能与数值结果完全一致

---

## 迭代2：效果执行抽象（为数据驱动做铺垫）

设计要点
- 定义轻量的效果描述与执行入口，但本迭代先不完全替换旧 `itemEffect`，只建立骨架并串起被动/主动调用点。

实施步骤
1) 数据描述（SO/类）
   - `EffectSpec`（临时类即可）：`string effectId`、`SerializableDictionary<string, float> params`
   - 在 `ItemData_Equipment` 暂留 `dynamicEffects[]`、`passvieEffects[]`，后续逐步迁移到 `EffectSpec[]`
2) 执行接口
   - `IEquipmentEffect`：`void Apply(EffectContext ctx)` / `void Remove(EffectContext ctx)`（或 `UseEffect` 语义）
   - `EffectContext`：`PlayerStats playerStats`、`Transform target`、`GameObject owner`、`float deltaTime` 等
3) 注册表
   - `EffectRegistry`：`Register(string effectId, Func<IEquipmentEffect>)`、`Create(effectId)`
   - 先内置“属性加成”算子，将 `Add/RemoveModifiers` 的逻辑收敛为 `AttributeModifierEffect`
4) 调用点改造
   - `Inventory.Equip/UnEquip` 调用注册表创建的效果实例执行（当前先只针对“属性加成”）
   - `PlayerStats` 中触发 `Armor` 被动时，通过注册表/管线执行（替代直接 `itemEffect` 调用的路径，或并行支持）

验收标准
- 不改变现有效果表现；可通过 effectId 定位执行器

---

## 迭代3：资源与配置迁移到 AssetBundle（AssetBundle Browser）

设计要点
- 用 `AssetBundles-Browser-master` 管理与构建 AB；将 `ItemData`、`itemEffect`、图标/模型/特效等资源纳入 AB，支持远程分发。

实施步骤
1) 资源标记与分组（在 AssetBundle Browser 中设置）
   - Bundle `items`：包含 `ItemData`（含 `ItemData_Equipment`）与其依赖（图标等）
   - Bundle `effects`：包含 `Effect`（`itemEffect` 子类或 `EffectSpec` 配置）
   - Bundle `art_common`/`fx_*`：美术资源按体积与频次分组
   - 命名规范：`group_name-platform`，避免跨平台冲突；必要时使用变体
2) 构建
   - 通过 AssetBundle Browser 一键构建目标平台 AB 与 `manifest`（含依赖关系与 hash）
   - 输出目录按 `platform/version/` 组织，保留 `manifest` 与 hash 文件
3) 远程分发
   - 将 AB 与 manifest 上传至 CDN/远端服务器；记录版本与 hash
4) 运行时加载管理器（新增 `AssetBundleManager`）
   - 支持：
     - 远程拉取 manifest（版本文件），比对本地缓存 hash，决定增量下载
     - 下载 AB（`UnityWebRequestAssetBundle.GetAssetBundle(url, hash, crc)`）并缓存
     - 依赖管理（先加载依赖，再加载主包）
     - 资源实例化/加载接口：`LoadScriptableObject<T>(bundle, path)`、`LoadSprite/Prefab` 等
   - 离线回退：当远程不可达时，优先使用本地已缓存或内置 StreamingAssets 包
5) 替换加载路径
   - 将 `Inventory.LoadItemDataFromResources()` 替换为：从 `AssetBundleManager` 读取 `ItemData` 列表（`items` 包）
   - 效果配置从 `effects` 包加载；按需在启动或首次进入背包界面时初始化

验收标准
- 本地/远程两种模式都能成功加载物品与效果资源
- 新物品通过远端下发后无需发包即可在 UI 与背包中出现

---

## 迭代4：新增装备与效果的“在线注册/加载”

设计要点
- 允许新增的 `ItemData_Equipment`、`EffectSpec`、`itemEffect` 子类以 AB 形式下发，并在运行时注册。

实施步骤
1) 运行时注册流程
   - 启动时（或触发更新）下载最新 manifest，比较版本 → 增量下载 `items`/`effects` 包
   - 从 `effects` 包中扫描 `EffectSpec` 或 `itemEffect` 子类（SO 资源），对每个 `effectId` 调用 `EffectRegistry.Register`
   - 从 `items` 包中加载 `ItemData_Equipment`，合并进 `itemDataBase`（或维护独立 `runtimeItemDataBase`）
2) 存档兼容
   - `GameData.inventory` 与 `equipmentId` 仍以 `itemId` 为主键
   - 加载存档时，如遇未知 `itemId`：尝试触发 AB 更新并重试加载；失败则忽略并告警
3) 校验与安全
   - 对 AB 与 manifest 做哈希校验；CRC 验证
   - effectId 白名单（仅执行已注册 effectId），未知效果不执行但保留物品

验收标准
- 能通过远端下发一个全新装备及其效果并在运行时可见、可装备、可生效

---

## 迭代5：`itemEffect` 向数据驱动 `EffectSpec` 的迁移（可选）

实施步骤
- 为常见效果提供算子节点：属性加成、护盾、治疗、DOT/HOT、概率/条件（击中/受击/时间）、冷却/叠层
- 在 `ItemData_Equipment` 中逐步从 `dynamicEffects[]`/`passvieEffects[]` 迁移为 `List<EffectSpec>`（SO 或 JSON 驱动）
- 资源迁移完成后，下线旧 `itemEffect` 路径

验收标准
- 策划可通过配置（而非代码）组合大多数常见效果

---

## 构建与发布（运维）
- 构建
  - 使用 AssetBundle Browser 构建目标平台 AB；输出 manifest/hash
  - HybridCLR（如需更新热更 dll），继续按既有 `StreamingAssets` 流程
- 发布
  - 将 AB 与 manifest 上传至 CDN；记录版本号（如 `version.json`）与各包 hash
  - 远端开关：灰度/回滚（服务端返回指定版本或禁用新包）
- 运行时
  - 版本检查：拉取 `version.json` → 对比本地 → 决定是否更新
  - 下载策略：先 manifest，再差异 AB；下载完成后替换生效
- 监控
  - 下载失败率、CRC/hash 校验失败、未注册 `effectId` 警告、存档丢失率

---

## 代码改造点（最小清单）
- `Inventory`
  - + `ApplyEquipmentStatModifiers(ItemData_Equipment)`、`RemoveEquipmentStatModifiers(ItemData_Equipment)`
  - `Equip/UnEquip` 替换为调用上述方法
  - 从 `AssetBundleManager` 初始化/刷新 `itemDataBase`
- `ItemData_Equipment`
  - `AddModifiers/RemoveModifiers` 标记 Deprecated；内部可改为委派至 `Inventory`（过渡期）
- 加载管理
  - 新增 `AssetBundleManager`：manifest 下载/解析、依赖加载、缓存、哈希/CRC 校验、离线回退
  - 替换 `Resources.LoadAll<ItemData>("Data/Item")` 为 AB 加载 API
- 存档
  - 保持以 `itemId` 为主键；加载时若缺失尝试 AB 更新并重试，再决定是否忽略

---

## 风险与缓解
- 远端资源不可用：本地回退（缓存或内置包）；下发“禁用新资源”开关
- 效果未注册：默认忽略并告警；不执行未知逻辑
- 资源/代码不一致：版本与哈希校验；灰度发布
- 依赖缺失：严格遵守 AssetBundle Browser 的依赖自动打包策略；上线前做整包校验

---

## 时间与里程碑（建议）
- 迭代0：1d（基线与用例）
- 迭代1：1-2d（Inventory集中应用与回归）
- 迭代2：2-3d（效果抽象骨架+属性加成算子）
- 迭代3：2-3d（AssetBundle Browser分组与运行时加载管理器）
- 迭代4：2d（在线注册与加载、校验与回滚）
- 迭代5：按需进行（算子库扩充与配置迁移）
