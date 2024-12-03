using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//2024.11.27到冬天了
public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "SampleScene";
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        //if (SaveManager.instance.HasSavedData() == false) //如果没有存档
        //    continueButton.SetActive(false);//不显示继续游戏按钮
    }


    public void ContinueGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Debug.Log("退出游戏");
    }
}