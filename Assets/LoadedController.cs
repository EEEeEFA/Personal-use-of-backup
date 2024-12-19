using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//2024.11.27�е�С�����
public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "SampleScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;//����UI_FadeScreen�ű�

    private void Start()
    {
        //if (SaveManager.instance.HasSavedData() == false) //���û�д浵
        //    continueButton.SetActive(false);//����ʾ������Ϸ��ť
    }


    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void ExitGame()
    {
        Debug.Log("�˳���Ϸ");
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)//���س�����Э��
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }
}