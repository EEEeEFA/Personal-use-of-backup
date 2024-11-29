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
    [SerializeField] public UI_ItemTooltip itemToolTip;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject ui_inGame;
    private void Start()
    {
        //Close(playerInput);
    }

    public void SwitchTo(GameObject _menu)
    {
        if (_menu != null)//����Ĳ˵���Ϊ��
        {
            for (int i = 0; i < transform.childCount; i++)//������ǰUI���������������
            {
                transform.GetChild(i).gameObject.SetActive(false);//����������������Ԫ��,ȷ��������ʾ�µ�UI����ʱ������������UI���涼�ᱻ����
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
                return;

        }
        _playerInput.SwitchCurrentActionMap("Player");
        SwitchTo(ui_inGame);


    }
}

