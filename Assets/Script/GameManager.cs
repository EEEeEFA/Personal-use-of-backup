using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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

    public void ReStart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(scene);
        Debug.Log("ReStart");
    }
}
