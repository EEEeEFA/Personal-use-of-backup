using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveManager
{
    public static GameManager instance;
    [SerializeField] private CheckPoint[] checkPoints;
    private void Awake()
    {
        if (instance != null && instance != this)//存在且不是这个
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 保持实例在场景切换时不被销毁
        }
    }

    public void ReStart()//重新加载本场景
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(scene);
        Debug.Log("ReStart");
    }

    public void LoadData(GameData _data)
    {
        foreach (CheckPoint checkPoint in checkPoints)//通过checkpointId一一对应存档与场景中的checkpoint,并激活存档时被激活的checkpoint
        {
            foreach(KeyValuePair<string, bool>savedCheckPoint in _data.chekcpoints)
            {
               if(checkPoint.checkpointId == savedCheckPoint.Key && savedCheckPoint.Value == true)
                {
                    checkPoint.ActivateCheckpoint();
                }

            }
        }
    }

    public void SaveData(ref GameData _data)//储存checkpoint状态
    {
        _data.chekcpoints.Clear();


        _data.closestCheckPointID = FindCloestCheckPoint().checkpointId;
        foreach (CheckPoint checkPoint in checkPoints)
        {
            _data.chekcpoints.Add(checkPoint.checkpointId, checkPoint.activated);
        }
    }

    private CheckPoint FindCloestCheckPoint()//找到最近的存档点
    {
        float closestDistance = Mathf.Infinity;
        CheckPoint closestPoint = null; 

        foreach(CheckPoint point in checkPoints)
        {
            if(point.activated == true)
            {
                float distance = Vector2.Distance(PlayerManager.instance.player.transform.position, point.transform.position)
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = point;   
                }
            }
        }
        return closestPoint;
    }
}
