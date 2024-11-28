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
    private void Start()
    {
        Close(playerInput);
    }
    public void Open(PlayerInput _playerInput)
    {
        if (gameObject != null)
            gameObject.SetActive(true);

        _playerInput.SwitchCurrentActionMap("UI");
        Debug.Log("Open");
    }

    public void Close(PlayerInput _playerInput)
    {
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    transform.GetChild(i).gameObject.SetActive(false);
        //}
        if (gameObject != null)
            gameObject.SetActive(false);

        _playerInput.SwitchCurrentActionMap("Player");
        Debug.Log("Close");
    }
}

