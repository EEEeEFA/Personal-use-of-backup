# UI Architecture

## 1. 模块简介
- **位置**: `Assets/Script/ArKnight/UI/` 以及 `Assets/Script/ArKnight/LuaScripts/UI/`
- **主要职责**: 管理游戏UI界面，包括主界面、干员界面、游戏界面等，采用C# + Lua混合架构
- **架构特点**: C#负责UI框架和复杂逻辑，Lua负责界面控制和数据展示

---

## 2. 脚本结构

### 2.1 C# UI框架层
```
UI/
├── UIBase.cs              # UI基类，定义通用UI生命周期
├── UIManager.cs           # UI管理器，负责UI的加载、显示、隐藏
├── RuaUI.cs               # Lua脚本启动器，桥接C#和Lua
└── Sub/                   # 复杂UI子模块
    ├── GameUI.cs          # 游戏主界面（战斗UI）
    ├── CharInfoUI.cs      # 干员详情界面
    ├── CharSelectUI.cs    # 干员选择界面
    ├── LoginUI.cs         # 登录界面
    ├── ShopUI.cs          # 商店界面
    ├── SettingUI.cs       # 设置界面
    ├── SquadUI.cs         # 编队界面
    ├── SelectDungeonUI.cs # 关卡选择界面
    ├── HouseUI.cs         # 宿舍界面
    ├── ItemInfoUI.cs      # 物品信息界面
    ├── CommonDialogUI.cs  # 通用对话框
    ├── GameWinUI.cs       # 游戏胜利界面
    └── GameLoseUI.cs      # 游戏失败界面
```

### 2.2 Lua UI控制层
```
LuaScripts/UI/
├── HomeUI.lua             # 主界面控制器
├── CharUI.lua             # 干员界面控制器
└── BasePanel.lua          # Lua UI基类
```

---

## 3. 核心类与组件

### 3.1 UI框架核心类

#### UIBase (抽象基类)
- **功能**: 所有UI的基类，定义通用UI生命周期
- **关键字段**: 
  - `internal string Name` - UI类型标识符
  - `internal UIState state` - UI状态管理
- **生命周期方法**: `Init()`, `Show()`, `Hide()`, `UpdateView()`
- **职责**: 提供UI基础功能，状态管理，资源清理

#### UIManager (单例管理器)
- **功能**: UI系统的中央管理器
- **核心方法**:
  - `Show(string type)` - 显示指定类型UI
  - `Hide(string type, bool destroy)` - 隐藏/销毁UI
  - `Load(string type)` - 动态加载UI预制体
  - `UnLoad(string type)` - 卸载UI资源
- **职责**: UI生命周期管理，资源加载，层级控制

#### RuaUI (Lua桥接器)
- **功能**: C#和Lua脚本的桥接组件
- **核心机制**:
  - 通过`Name`字段确定加载的Lua脚本
  - 使用XLua框架实现C#和Lua交互
  - 提供Lua脚本的注入机制
- **生命周期绑定**: 将Lua函数绑定到C#生命周期方法

### 3.2 复杂UI子模块

#### GameUI (游戏主界面)
- **功能**: 战斗场景UI，包含血量条、费用系统、干员卡片
- **关键组件**: 
  - `PlacePanel` - 干员放置面板
  - `CharCard` - 干员卡片列表
  - `HealthBar` - 血量条系统
- **职责**: 战斗UI管理，干员部署，状态显示

#### CharInfoUI (干员详情)
- **功能**: 干员详细信息展示，支持滑动浏览
- **特性**: 支持多干员信息切换，职业帮助系统
- **职责**: 干员数据展示，详细信息管理

#### CharSelectUI (干员选择)
- **功能**: 干员选择界面，支持筛选和排序
- **职责**: 干员选择逻辑，数据筛选

### 3.3 Lua UI控制器

#### HomeUI (主界面Lua控制器)
- **功能**: 主界面UI管理，显示玩家信息、游戏资源、时间等
- **生命周期方法**: `show()`, `hide()`, `update()`, `updateView()`
- **特性**: 视差效果，实时时间显示，背景音乐管理
- **调用关系**: 调用PlayerManager获取玩家数据，调用SoundManager播放音乐

#### CharUI (干员界面Lua控制器)
- **功能**: 干员列表界面管理，处理卡片创建、排序和交互
- **生命周期方法**: `show()`, `hide()`, `updateView()`, `init()`
- **核心功能**: 干员卡片创建，等级/稀有度排序，卡片交互
- **调用关系**: 调用PlayerManager获取干员数据，调用CharManager获取干员信息

### 3.4 数据结构

#### CharCard (Lua自定义对象)
- **功能**: 干员卡片数据容器，包含干员信息和UI组件引用
- **用途**: 被CharUI用于管理干员卡片显示
- **方法**: `setData()`, `update()`, `setActive()`

---

## 4. 工作流程

### 4.1 UI加载流程
1. **调用显示**: `UIManager.Show("HomeUI")`
2. **检查缓存**: 如果UI不存在，调用`Load("HomeUI")`
3. **创建GameObject**: 实例化UI预制体
4. **设置Name**: `ui.Name = "HomeUI"`
5. **初始化**: 调用`ui.Init()`
6. **Lua脚本加载**: 如果是RuaUI，加载对应的Lua脚本
7. **显示UI**: 调用`ui.Show()`和`ui.UpdateView()`

### 4.2 Lua UI工作流程
1. **HomeUI在show()时**: 初始化界面，播放背景音乐，获取玩家数据
2. **HomeUI在update()中**: 实时更新时间显示和鼠标视差效果
3. **HomeUI在updateView()中**: 刷新玩家信息显示
4. **CharUI在init()中**: 设置排序按钮的事件监听
5. **CharUI在updateView()中**: 创建/更新干员卡片，应用排序算法
6. **CharUI在SetSibling()中**: 调整卡片显示顺序

### 4.3 C# UI工作流程
1. **GameUI初始化**: 创建血量条、干员卡片、放置面板
2. **战斗状态管理**: 实时更新血量、费用、击杀数
3. **干员部署**: 通过PlacePanel处理干员放置逻辑
4. **状态同步**: 与游戏逻辑同步UI状态

---

## 5. 依赖关系

### 5.1 内部依赖
- **UI框架**: UIManager → UIBase → RuaUI
- **Lua UI**: HomeUI → PlayerManager, SoundManager, UIManager
- **Lua UI**: CharUI → PlayerManager, CharManager, CharInfoUI
- **复杂UI**: GameUI → PlayerData, Dungeon, CharCard, HealthBar
- **数据展示**: CharInfoUI → PlayerData, CharPrefab

### 5.2 外部依赖
- **动画系统**: DOTween动画系统
- **UI系统**: Unity UI系统
- **Lua框架**: XLua热更新框架
- **资源管理**: Asset加载系统

### 5.3 数据流
- **玩家数据**: PlayerManager → UI显示
- **干员数据**: CharManager → CharUI/CharInfoUI
- **游戏状态**: Dungeon → GameUI
- **音频资源**: SoundManager → UI音效

---

## 6. 架构特点

### 6.1 混合架构优势
- **C#负责**: 复杂逻辑、性能关键部分、框架管理
- **Lua负责**: 界面控制、数据展示、快速迭代
- **桥接机制**: RuaUI提供C#和Lua的无缝交互

### 6.2 扩展性设计
- **模块化**: 每个UI独立管理，便于维护
- **热更新**: Lua脚本支持热更新，快速修复和功能迭代
- **资源管理**: 统一的资源加载和释放机制

### 6.3 性能优化
- **对象池**: UI组件复用，减少GC压力
- **懒加载**: UI按需加载，节省内存
- **状态管理**: 精确的UI状态控制，避免不必要的更新