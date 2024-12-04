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
        if (instance != null && instance != this)//�����Ҳ������
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ����ʵ���ڳ����л�ʱ��������
        }
    }

    public void ReStart()//���¼��ر�����
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(scene);
        Debug.Log("ReStart");
    }

    public void LoadData(GameData _data)
    {
        foreach (CheckPoint checkPoint in checkPoints)//ͨ��checkpointIdһһ��Ӧ�浵�볡���е�checkpoint,������浵ʱ�������checkpoint
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

    public void SaveData(ref GameData _data)//����checkpoint״̬
    {
        _data.chekcpoints.Clear();


        _data.closestCheckPointID = FindCloestCheckPoint().checkpointId;
        foreach (CheckPoint checkPoint in checkPoints)
        {
            _data.chekcpoints.Add(checkPoint.checkpointId, checkPoint.activated);
        }
    }

    private CheckPoint FindCloestCheckPoint()//�ҵ�����Ĵ浵��
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
