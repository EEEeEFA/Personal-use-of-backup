using Autodesk.Fbx;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class UIPanelBinding
{
    public GameObject panel;
    public InputActionReference openAction;
    public InputActionReference closeAction;
    
    [Tooltip("在Inspector中显示该绑定的说明")]
    public string description;  // 可选，帮助在Inspector中识别每个绑定
}

public class UI : MonoBehaviour
{
    [Header("End screens")]
    public UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject ReSpawnBotton;
    [Space]

    [SerializeField] public UI_ItemTooltip itemToolTip;
    [SerializeField] private GameObject ui_inGame;
    [SerializeField] private GameObject ui_fadeScreen;

    [Header("UI Panel Bindings")]
    [Tooltip("配置每个UI面板的打开和关闭动作")]
    public List<UIPanelBinding> panelBindings;

    private PlayerInput playerInput;
    private Dictionary<InputAction, System.Action<InputAction.CallbackContext>> _registeredActions 
        = new Dictionary<InputAction, System.Action<InputAction.CallbackContext>>();

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        foreach (var binding in panelBindings)
        {
            if (binding.panel == null || binding.openAction == null || binding.closeAction == null)
            {
                Debug.LogWarning($"UI绑定配置不完整: {binding.description ?? "未知绑定"}");
                continue;
            }

            // 注册打开动作
            var openCallback = (InputAction.CallbackContext ctx) => SwitchWithKeyTo(binding.panel);
            binding.openAction.action.performed += openCallback;
            _registeredActions[binding.openAction.action] = openCallback;

            // 注册关闭动作
            var closeCallback = (InputAction.CallbackContext ctx) => SwitchWithKeyTo(binding.panel);
            binding.closeAction.action.performed += closeCallback;
            _registeredActions[binding.closeAction.action] = closeCallback;
        }
    }

    private void OnDisable()
    {
        foreach (var binding in panelBindings)
        {
            if (binding.openAction != null && 
                _registeredActions.TryGetValue(binding.openAction.action, out var openCallback))
            {
                binding.openAction.action.performed -= openCallback;
            }
            
            if (binding.closeAction != null && 
                _registeredActions.TryGetValue(binding.closeAction.action, out var closeCallback))
            {
                binding.closeAction.action.performed -= closeCallback;
            }
        }
        _registeredActions.Clear();
    }

    private void Start()
    {
        SwitchTo(ui_inGame);
        fadeScreen.gameObject.SetActive(true);
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

    public void SwithToFade() => SwitchTo(ui_fadeScreen);
}

