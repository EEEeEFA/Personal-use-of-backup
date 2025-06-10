推荐技术栈：仿蛊 (Fang Gu)
本文档概述了使用Unity引擎开发《仿蛊》的推荐技术栈与工具选择，这些选择均针对游戏设计文档(GDD)中描述的核心特性进行了专门优化，包括2D瓦片世界、复杂的蛊术释放系统以及混合存档模式。

核心引擎与基础配置
引擎：Unity 2022.3 LTS（或更新的长期支持版本）
选型依据：长期支持版本能为多年期项目提供最佳稳定性支持，避免非LTS版本升级导致的兼容性问题。

主编程语言：C#
选型依据：Unity原生脚本语言。

版本控制：Git
代码托管：GitHub或GitLab
Git LFS（大文件存储）：必须启用。用于追踪纹理、音频、精灵等大型二进制文件，确保仓库运行效率。
Unity .gitignore：使用标准模板避免提交临时文件和机器特定配置。

项目管理：Trello/HacknPlan/Jira
选型依据：HacknPlan专为游戏开发工作流设计，可高效管理任务、缺陷与设计构思。

Unity功能模块与系统
2.1 图形与场景构建
渲染管线：2D URP（通用渲染管线）
选型依据：专为2D优化，支持2D光照氛围营造，无需编写代码即可通过Shader Graph实现自定义视觉效果。

地图创建：2D Tilemap Editor（Unity官方包）
选型依据：最高效的网格化地图构建方案，完美契合GDD设计需求，且直接集成在编辑器中。

2.2 玩法与角色逻辑
输入系统：Input System（Unity官方包）
选型依据：旧输入管理器无法满足复杂蛊术释放需求。新系统支持"操作映射表"（Action Maps），可快速切换"家园/战斗/蛊术UI"等控制模式——这正是玩家输入咒文时所需的核心功能。

数据管理：ScriptableObjects
选型依据：避免硬编码，所有游戏数据均可通过该方案定义：
• 蛊术：每个蛊术作为ScriptableObject存储名称、描述、类型及效果逻辑
• 角色天赋：定义天赋效果与属性
• 物品装备：创建全物品模板
该方案使设计师能独立完成数值平衡，无需程序员介入修改代码。

2.3 用户界面系统
UI系统：
UI Toolkit
适用场景：背包/设置等需要动态更新的平面界面（Screen Space Overlay）
优势：轻量化、支持XML/CSS式复杂布局、便于运行时动态生成内容

UGUI（Unity UI）
适用场景：3D场景中的血条/交互提示等世界空间UI（World Space）
优势：通过RectTransform与场景直接交互，完美适配3D空间

混合存档系统
根据需求文档，本系统需同时实现自动存档与手动存档槽功能。

序列化库：Newtonsoft Json.NET（通过包管理器安装）
选型依据：Unity内置JsonUtility虽快但功能受限（难以处理字典/复杂嵌套类型）。Newtonsoft作为.NET生态JSON处理标准方案，可完美支持任意数据结构。

实现策略：
• 创建数据容器类：用[System.Serializable]标记的C#类存储需存档数据

csharp
[System.Serializable]
public class PlayerData {
    public int level;
    public float currentExp;
    public Vector3 lastPosition;
    // ...其他玩家属性
}

[System.Serializable]
public class GameState {
    public PlayerData playerData;
    public InventoryData inventoryData;
    public WorldData worldData;
    // ...其他游戏状态
}
• 构建存档管理器：通过静态类或单例模式实现核心逻辑

SaveGame(int slotIndex)：收集游戏数据→封装为GameState对象→用Newtonsoft序列化为JSON字符串→写入文件

LoadGame(int slotIndex)：读取JSON文件→反序列化为GameState对象→将数据分发至各游戏系统

AutoSave()：调用SaveGame(0)专用于自动存档槽

文件路径：使用Application.persistentDataPath获取跨平台可写目录（如.../save_slot_1.json）

自动存档触发点：
• 进入新地图/传送时
• 完成主线任务目标后
• 定时触发（如每10分钟）
