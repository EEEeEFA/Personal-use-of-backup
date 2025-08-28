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
    
    [Tooltip("��Inspector����ʾ�ð󶨵�˵��")]
    public string description;  
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
    [Tooltip("����ÿ��UI���Ĵ򿪺͹رն���")]
    public List<UIPanelBinding> panelBindings;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Dictionary<InputAction, System.Action<InputAction.CallbackContext>> _registeredActions 
        = new Dictionary<InputAction, System.Action<InputAction.CallbackContext>>();

    // private void Awake()
    // {
    //     playerInput = GetComponent<PlayerInput>();
    // }

    private void OnEnable()
    {
        foreach (var binding in panelBindings)
        {
            if (binding.panel == null || binding.openAction == null || binding.closeAction == null)
            {
                Debug.LogWarning($"UI�����ò�����: {binding.description ?? "δ֪��"}");
                continue;
            }

            System.Action<InputAction.CallbackContext> openCallback = ctx => SwitchWithKeyTo(binding.panel);
            binding.openAction.action.performed += openCallback;
            _registeredActions[binding.openAction.action] = openCallback;

            System.Action<InputAction.CallbackContext> closeCallback = ctx => SwitchWithKeyTo(binding.panel);
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
        if (_menu != null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
                if (fadeScreen == false)
                    transform.GetChild(i).gameObject.SetActive(false);
            }

            _menu.SetActive(true);

            if (GameManager.instance != null)
            {
                if (_menu == ui_inGame)
                    GameManager.instance.PauseGame(false);
                else
                    GameManager.instance.PauseGame(true);
            }

        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
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


    private void CheckForInGameUI(PlayerInput _playerInput)
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

