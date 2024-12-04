
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024.11.25 存档数据存放脚本
[System.Serializable]
public class GameData
{
    public int currency;

    public SerializableDictionary<string, int> inventory;

    public List<string> equipmentId;

    public SerializableDictionary<string, bool> chekcpoints;
    public string closestCheckPointID;

    public GameData()
    {
        this.currency = 0;
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        chekcpoints = new SerializableDictionary<string, bool>();
        closestCheckPointID = string.Empty;
    }

}