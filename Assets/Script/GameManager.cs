using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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

    public void ReStart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(scene);
        Debug.Log("ReStart");
    }
}
