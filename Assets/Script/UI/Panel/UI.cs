using Autodesk.Fbx;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI : MonoBehaviour
{
    [Header("End screens")]
    public UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject ReSpawnBotton;
    [Space]

    [SerializeField] public UI_ItemTooltip itemToolTip;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject ui_inGame;
    private void Start()
    {
        SwitchTo(ui_inGame);
    }   

    public void SwitchTo(GameObject _menu)
    {
        if (_menu != null)//����Ĳ˵���Ϊ��
        {
            for (int i = 0; i < transform.childCount; i++)//������ǰUI���������������
            {
                bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;//���UI�����Ƿ���FadeScreens
                if (fadeScreen == false)
                    transform.GetChild(i).gameObject.SetActive(false);//����������������Ԫ��,ȷ��������ʾ�µ�UI����ʱ������������UI���涼�ᱻ����
                    Debug.Log("����"+transform.GetChild(i).gameObject);
            }

            _menu.SetActive(true);//��ʾ

        }
    }

    public void SwitchWithKeyTo(GameObject _menu)//�����л�UI���߼�
    {
        if (_menu != null && _menu.activeSelf)// UI�����Ѿ���ʾ������, ���Ŀ��UI����δ��ʾ������ SwitchTo ��ʾ��
        {
            _menu.SetActive(false);
            CheckForInGameUI(playerInput);
            Debug.Log("InGameUI");
            Debug.Log(_menu);
            return;
        }
        playerInput.SwitchCurrentActionMap("UI");
        SwitchTo(_menu);
    }


    private void CheckForInGameUI(PlayerInput _playerInput)//�ر�����UI����ص�InGameUI
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                Debug.Log(transform.GetChild(i).gameObject);
                return;
            }


        }
        _playerInput.SwitchCurrentActionMap("Player");
        SwitchTo(ui_inGame);


    }
    public void SwitchOnEndScreen()
    {
        SwitchTo(null);
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(.5f);
        ReSpawnBotton.SetActive(true);

    }

    public void RestartGame() => GameManager.instance.ReStart();
}

