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
        if (_menu != null)//传入的菜单不为空
        {
            for (int i = 0; i < transform.childCount; i++)//遍历当前UI对象的所有子物体
            {
                bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
                if (fadeScreen == false)
                    transform.GetChild(i).gameObject.SetActive(false);//遍历并隐藏除了黑幕之外的所有Object,确保了在显示新的UI界面时，所有其他的UI界面都会被隐藏
            }

            _menu.SetActive(true);//显示

            if (GameManager.instance != null)//打开UI时暂停游戏
            {
                if (_menu == ui_inGame)
                    GameManager.instance.PauseGame(false);
                else
                    GameManager.instance.PauseGame(true);
            }

        }
    }

    public void SwitchWithKeyTo(GameObject _menu)//处理切换UI的逻辑
    {
        if (_menu != null && _menu.activeSelf)// UI界面已经显示，隐藏, 如果目标UI界面未显示，调用 SwitchTo 显示。
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


    private void CheckForInGameUI(PlayerInput _playerInput)//关闭其他UI都会回到InGameUI
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
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

