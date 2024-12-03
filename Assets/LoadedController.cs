using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//2024.11.27��������
public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "SampleScene";
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        //if (SaveManager.instance.HasSavedData() == false) //���û�д浵
        //    continueButton.SetActive(false);//����ʾ������Ϸ��ť
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
        Debug.Log("�˳���Ϸ");
    }
}