using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public Player player;

    public int currency;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); // 保持实例在场景切换时不被销毁
        }
    }

    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
        {
            Debug.Log("Not enough money");
            return false;
        }

        currency -= _price;
        return true;
    }

    public int CurrentCurrencyAmount() => currency;

    void ISaveManager.LoadData(GameData _data)
    {
        currency = _data.currency;
    }

    void ISaveManager.SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }
}
