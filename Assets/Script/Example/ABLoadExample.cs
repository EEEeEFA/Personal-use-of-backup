using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// AB包加载示例
/// 展示如何使用ABUpdateMgr和ABMgr来智能加载AB包
/// </summary>
public class ABLoadExample : MonoBehaviour
{
    [Header("测试配置")]
    public string testABName = "testab"; // 要测试的AB包名称
    public string testResourceName = "TestPrefab"; // AB包中的资源名称

    void Start()
    {
        // 示例1：检查AB包是否需要下载
        CheckABDownloadStatus();
        
        // 示例2：检查AB包是否存在
        CheckABExists();
        
        // 示例3：加载AB包中的资源
        LoadABResource();
    }

    /// <summary>
    /// 检查AB包是否需要下载
    /// </summary>
    void CheckABDownloadStatus()
    {
        if (ABMgr.Instance.IsABNeedDownload(testABName))
        {
            Debug.Log($"AB包 {testABName} 需要下载");
        }
        else
        {
            Debug.Log($"AB包 {testABName} 不需要下载，使用本地版本");
        }
    }

    /// <summary>
    /// 检查AB包是否存在
    /// </summary>
    void CheckABExists()
    {
        if (ABMgr.Instance.IsABExists(testABName))
        {
            Debug.Log($"AB包 {testABName} 存在");
        }
        else
        {
            Debug.Log($"AB包 {testABName} 不存在");
        }
    }

    /// <summary>
    /// 加载AB包中的资源
    /// </summary>
    void LoadABResource()
    {
        // 首先检查AB包是否存在
        if (!ABMgr.Instance.IsABExists(testABName))
        {
            Debug.LogError($"AB包 {testABName} 不存在，无法加载资源");
            return;
        }

        // 同步加载资源
        Object loadedResource = ABMgr.Instance.LoadRes(testABName, testResourceName);
        if (loadedResource != null)
        {
            Debug.Log($"成功加载资源: {testResourceName}");
        }
        else
        {
            Debug.LogError($"加载资源失败: {testResourceName}");
        }

        // 异步加载资源示例
        ABMgr.Instance.LoadResAsync(testABName, testResourceName, (loadedObj) =>
        {
            if (loadedObj != null)
            {
                Debug.Log($"异步加载资源成功: {testResourceName}");
            }
            else
            {
                Debug.LogError($"异步加载资源失败: {testResourceName}");
            }
        });
    }

    /// <summary>
    /// 完整的AB包更新和加载流程示例
    /// </summary>
    public void CompleteABUpdateAndLoadExample()
    {
        // 1. 首先进行AB包更新检查
        ABUpdateMgr.Instance.CheckUpdate(
            (isUpdateSuccess) =>
            {
                if (isUpdateSuccess)
                {
                    Debug.Log("AB包更新完成，开始加载资源");
                    // 2. 更新完成后加载资源
                    LoadABResource();
                }
                else
                {
                    Debug.LogError("AB包更新失败");
                }
            },
            (updateInfo) =>
            {
                Debug.Log($"更新进度: {updateInfo}");
            }
        );
    }

    /// <summary>
    /// 获取AB包加载路径信息
    /// </summary>
    public void GetABPathInfo()
    {
        if (ABUpdateMgr.Instance != null)
        {
            string loadPath = ABUpdateMgr.Instance.GetABLoadPath(testABName);
            bool needDownload = ABUpdateMgr.Instance.IsABNeedDownload(testABName);
            
            Debug.Log($"AB包 {testABName} 的加载路径: {loadPath}");
            Debug.Log($"AB包 {testABName} 是否需要下载: {needDownload}");
        }
    }
} 