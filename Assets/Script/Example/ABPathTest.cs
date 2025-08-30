using UnityEngine;
using System.IO;

/// <summary>
/// AB包路径测试脚本
/// 用于验证AB包路径判断逻辑是否正确
/// </summary>
public class ABPathTest : MonoBehaviour
{
    [Header("测试配置")]
    public string testABName = "testab"; // 要测试的AB包名称

    void Start()
    {
        TestABPathLogic();
    }

    /// <summary>
    /// 测试AB包路径判断逻辑
    /// </summary>
    void TestABPathLogic()
    {
        Debug.Log("=== AB包路径判断逻辑测试 ===");
        
        // 测试1：检查AB包是否存在
        bool existsInPersistent = File.Exists(Application.persistentDataPath + "/" + testABName);
        bool existsInStreaming = File.Exists(Application.streamingAssetsPath + "/" + testABName);
        
        Debug.Log($"AB包 {testABName} 在persistentDataPath中存在: {existsInPersistent}");
        Debug.Log($"AB包 {testABName} 在streamingAssetsPath中存在: {existsInStreaming}");
        
        // 测试2：使用ABUpdateMgr检查路径
        if (ABUpdateMgr.GetInstance() != null)
        {
            string loadPath = ABUpdateMgr.GetInstance().GetABLoadPath(testABName);
            bool needDownload = ABUpdateMgr.GetInstance().IsABNeedDownload(testABName);
            
            Debug.Log($"ABUpdateMgr - 加载路径: {loadPath}");
            Debug.Log($"ABUpdateMgr - 需要下载: {needDownload}");
        }
        
        // 测试3：使用ABMgr检查路径
        if (ABLoadMgr.GetInstance() != null)
        {
            string loadPath = ABLoadMgr.GetInstance().GetABPath(testABName);
            
            Debug.Log($"ABMgr - 加载路径: {loadPath}");
        }
        
        // 测试4：模拟不同场景
        //TestDifferentScenarios();
    }

//     /// <summary>
//     /// 测试不同场景下的路径判断
//     /// </summary>
//     void TestDifferentScenarios()
//     {
//         Debug.Log("\n=== 不同场景测试 ===");
        
//         // 场景1：AB包只在streamingAssetsPath中存在
//         TestScenario("local_only", false, true, "应该从streamingAssetsPath加载");
        
//         // 场景2：AB包只在persistentDataPath中存在
//         TestScenario("downloaded_only", true, false, "应该从persistentDataPath加载");
        
//         // 场景3：AB包在两个路径中都存在
//         TestScenario("both_exist", true, true, "应该优先从persistentDataPath加载");
        
//         // 场景4：AB包在两个路径中都不存在
//         TestScenario("not_exist", false, false, "AB包不存在");
//     }

//     /// <summary>
//     /// 测试特定场景
//     /// </summary>
//     void TestScenario(string abName, bool inPersistent, bool inStreaming, string expectedBehavior)
//     {
//         Debug.Log($"\n场景: {abName} - {expectedBehavior}");
        
//         // 模拟文件存在情况
//         string persistentPath = Application.persistentDataPath + "/" + abName;
//         string streamingPath = Application.streamingAssetsPath + "/" + abName;
        
//         // 注意：这里只是模拟，实际文件可能不存在
//         Debug.Log($"persistentDataPath: {persistentPath}");
//         Debug.Log($"streamingAssetsPath: {streamingPath}");
        
//         if (ABUpdateMgr.Instance != null)
//         {
//             string actualPath = ABUpdateMgr.Instance.GetABLoadPath(abName);
//             bool needDownload = ABUpdateMgr.Instance.IsABNeedDownload(abName);
            
//             Debug.Log($"实际加载路径: {actualPath}");
//             Debug.Log($"需要下载: {needDownload}");
            
//             // 验证逻辑是否正确
//             bool isCorrect = false;
//             if (inPersistent)
//             {
//                 isCorrect = actualPath.Contains("persistentDataPath") && !needDownload;
//             }
//             else if (inStreaming)
//             {
//                 isCorrect = actualPath.Contains("streamingAssetsPath") && !needDownload;
//             }
//             else
//             {
//                 isCorrect = actualPath.Contains("streamingAssetsPath"); // 默认路径
//             }
            
//             Debug.Log($"逻辑验证: {(isCorrect ? "正确" : "错误")}");
//         }
//     }

//     /// <summary>
//     /// 手动测试AB包加载
//     /// </summary>
//     [ContextMenu("测试AB包加载")]
//     public void TestABLoad()
//     {
//         if (ABLoadMgr.GetInstance() != null && ABLoadMgr.GetInstance().IsABExists(testABName))
//         {
//             Debug.Log($"尝试加载AB包: {testABName}");
//             try
//             {
//                 // 尝试加载AB包（不加载具体资源，只加载AB包本身）
//                 ABMgr.Instance.LoadAB(testABName);
//                 Debug.Log("AB包加载成功");
//             }
//             catch (System.Exception e)
//             {
//                 Debug.LogError($"AB包加载失败: {e.Message}");
//             }
//         }
//         else
//         {
//             Debug.LogWarning($"AB包 {testABName} 不存在，无法测试加载");
//         }
//     }
     } 