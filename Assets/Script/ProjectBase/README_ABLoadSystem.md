# AB包智能加载系统

## 概述

这个系统实现了AB包的智能加载功能，能够根据AB包的更新状态自动选择从本地路径（`streamingAssetsPath`）或下载路径（`persistentDataPath`）加载AB包。

## 核心功能

### 1. AB包更新检测 (ABUpdateMgr)

- **CheckUpdate()**: 检测AB包更新，对比本地和远程AB包信息
- **IsABNeedDownload()**: 检查指定AB包是否需要下载
- **GetABLoadPath()**: 获取AB包的实际加载路径
- **IsABExists()**: 检查AB包是否存在

### 2. AB包加载管理 (ABMgr)

- **LoadAB()**: 加载AB包（自动选择正确路径）
- **LoadRes()**: 同步加载AB包中的资源
- **LoadResAsync()**: 异步加载AB包中的资源
- **IsABExists()**: 检查AB包是否存在
- **IsABNeedDownload()**: 检查AB包是否需要下载

## 工作流程

### 1. AB包对比流程

```
1. 下载远程AB包对比文件
2. 解析远程AB包信息
3. 加载本地AB包对比文件
4. 对比本地和远程AB包信息
5. 生成需要下载的AB包列表
6. 删除无用的AB包
7. 下载需要更新的AB包
8. 更新本地对比文件
```

### 2. AB包加载流程

```
1. 检查AB包是否需要下载
2. 如果需要下载且已下载完成 → 从persistentDataPath加载
3. 如果不需要下载 → 从streamingAssetsPath加载
4. 加载AB包及其依赖包
5. 从AB包中加载指定资源
```

## 使用方法

### 基本使用

```csharp
// 1. 检查AB包更新
ABUpdateMgr.Instance.CheckUpdate(
    (isSuccess) => {
        if (isSuccess) {
            Debug.Log("AB包更新完成");
        }
    },
    (progress) => {
        Debug.Log($"更新进度: {progress}");
    }
);

// 2. 加载AB包中的资源
Object resource = ABMgr.Instance.LoadRes("myab", "MyPrefab");
```

### 高级使用

```csharp
// 检查AB包是否需要下载
if (ABMgr.Instance.IsABNeedDownload("myab")) {
    Debug.Log("AB包需要下载");
} else {
    Debug.Log("AB包使用本地版本");
}

// 检查AB包是否存在
if (ABMgr.Instance.IsABExists("myab")) {
    Debug.Log("AB包存在");
} else {
    Debug.Log("AB包不存在");
}

// 获取AB包加载路径
string path = ABUpdateMgr.Instance.GetABLoadPath("myab");
Debug.Log($"AB包加载路径: {path}");
```

### 完整示例

```csharp
public class ABLoadExample : MonoBehaviour
{
    void Start()
    {
        // 完整的AB包更新和加载流程
        ABUpdateMgr.Instance.CheckUpdate(
            (isUpdateSuccess) => {
                if (isUpdateSuccess) {
                    // 更新完成后加载资源
                    LoadMyResource();
                }
            },
            (updateInfo) => {
                Debug.Log($"更新进度: {updateInfo}");
            }
        );
    }

    void LoadMyResource()
    {
        // 检查AB包是否存在
        if (ABMgr.Instance.IsABExists("myab")) {
            // 同步加载
            GameObject prefab = ABMgr.Instance.LoadRes<GameObject>("myab", "MyPrefab");
            
            // 异步加载
            ABMgr.Instance.LoadResAsync<GameObject>("myab", "MyPrefab", (loadedPrefab) => {
                if (loadedPrefab != null) {
                    Debug.Log("资源加载成功");
                }
            });
        }
    }
}
```

## 性能优化

### 路径判断优化

系统使用 `HashSet<string>` 来存储已下载的AB包列表，提供 O(1) 时间复杂度的查找性能：

```csharp
// 高效的路径判断
if (downloadedABList.Contains(abName))  // O(1) HashSet查找
{
    return Application.persistentDataPath + "/" + abName;
}
```

### 性能对比

| 方法 | 时间复杂度 | 适用场景 | 性能 |
|------|------------|----------|------|
| `downloadedABList.Contains()` | O(1) | 已下载AB包检查 | 最快 |
| `File.Exists()` | O(1) | 文件系统检查 | 慢（磁盘I/O） |

### 内存管理

- **downLoadList**: 存储待下载的AB包，下载完成后会被清空
- **downloadedABList**: 存储已下载的AB包，长期保持，提供快速查找

## 路径优先级

1. **persistentDataPath** (下载的AB包) - 最高优先级
2. **streamingAssetsPath** (本地AB包) - 备用路径

### 路径判断逻辑

系统使用以下逻辑来判断AB包应该从哪个路径加载：

1. **如果AB包在已下载列表中** → 从persistentDataPath加载（已下载的AB包）
2. **否则** → 从streamingAssetsPath加载（本地AB包）

这种逻辑确保了：
- 已经下载完成的AB包会从下载路径加载
- 从未下载过的AB包会从本地路径加载
- 正在下载的AB包在下载完成后会自动添加到已下载列表

**注意**: `downLoadList`（待下载列表）和 `downloadedABList`（已下载列表）是互斥的，所以只需要判断已下载列表即可。

## 注意事项

1. **依赖包处理**: 系统会自动处理AB包的依赖关系，确保依赖包也使用正确的路径加载
2. **主包加载**: 主包（包含AssetBundleManifest）也会根据实际情况选择正确的加载路径
3. **错误处理**: 如果AB包不存在，系统会返回null或调用失败回调
4. **内存管理**: 使用完毕后记得调用`UnLoad()`或`ClearAB()`释放内存

## 配置说明

### 服务器配置
在`ABUpdateMgr.cs`中修改以下配置：
```csharp
private string serverIP = "ftp://192.168.31.178"; // FTP服务器地址
```

### 平台配置
系统会自动根据平台选择正确的AB包名称：
- **PC**: "PC"
- **Android**: "Android"  
- **iOS**: "IOS"

## 扩展功能

### 自定义路径逻辑
如果需要自定义AB包路径选择逻辑，可以继承`ABMgr`类并重写`GetABPath()`方法：

```csharp
public class CustomABMgr : ABMgr
{
    protected override string GetABPath(string abName)
    {
        // 自定义路径选择逻辑
        return customPath;
    }
}
```

### 添加新的检查方法
可以在`ABUpdateMgr`中添加新的检查方法：

```csharp
public bool IsABUpToDate(string abName)
{
    // 检查AB包是否为最新版本
    return !IsABNeedDownload(abName);
}
``` 