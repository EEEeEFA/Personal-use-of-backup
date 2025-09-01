# UI Architecture

## 1. 模块简介
- **位置**: `Assets/Script/ArKnight/LuaScripts/UI/`
- **主要职责**: 管理游戏UI界面，包括主界面和干员界面的显示、交互和数据处理

---

## 2. 脚本结构
UI/
- HomeUI.lua.txt   # 主界面控制器，管理玩家信息显示和视差效果
- CharUI.lua.txt   # 干员界面控制器，管理干员卡片列表和排序

---

## 3. 核心类与组件

### 3.1 主要UI控制器
- **HomeUI : UI.RuaUI (HomeUI.lua.txt)**
  - 功能: 主界面UI管理，显示玩家信息、游戏资源、时间等
  - 生命周期方法: show(), hide(), update(), updateView()
  - 调用关系: 调用PlayerManager获取玩家数据，调用SoundManager播放音乐

- **CharUI : UI.RuaUI (CharUI.lua.txt)**
  - 功能: 干员列表界面管理，处理卡片创建、排序和交互
  - 生命周期方法: show(), hide(), updateView(), init()
  - 调用关系: 调用PlayerManager获取干员数据，调用CharManager获取干员信息

### 3.2 数据结构
- **CharCard : 自定义对象 (CharUI.lua.txt)**
  - 功能: 干员卡片数据容器，包含干员信息和UI组件引用
  - 用途: 被CharUI用于管理干员卡片显示

### 3.3 工具类 / 辅助类
- **排序算法 (CharUI.lua.txt)**
  - 功能: 提供等级排序和稀有度排序两种算法
  - 方法: sortLevel(), sortRarity()

---

## 4. 工作流程
1. HomeUI在show()时初始化界面，播放背景音乐，获取玩家数据
2. HomeUI在update()中实时更新时间显示和鼠标视差效果
3. HomeUI在updateView()中刷新玩家信息显示
4. CharUI在init()中设置排序按钮的事件监听
5. CharUI在updateView()中创建/更新干员卡片，应用排序算法
6. CharUI在SetSibling()中调整卡片显示顺序

---

## 5. 依赖关系
- **内部依赖**: HomeUI → PlayerManager, SoundManager, UIManager
- **内部依赖**: CharUI → PlayerManager, CharManager, CharInfoUI
- **外部依赖**: DOTween动画系统, Unity UI系统

---