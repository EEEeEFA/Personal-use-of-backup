
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024.11.25
[System.Serializable]
public class GameData
{
    public int currency;

    public SerializableDictionary<string, int> inventory;

    public GameData()
    {
        this.currency = 0;
        inventory = new SerializableDictionary<string, int>();
    }

}