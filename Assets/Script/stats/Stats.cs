using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//�����������ô�����Ľű��Ĳ������ڱ�Ľű���inspetor����ʾ����
public class Stats
{
    [SerializeField] private int baseValue;
    public List<int> modifiers;
    public int GetValue()
    {
        int finalValue = baseValue;
        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }
        return finalValue;
    }

    public void AddModifier(int _modifer)
    {
        modifiers.Add(_modifer);
    }

    public void RemoveModifier(int _modifer)
    {
        modifiers.Remove(_modifer);
    }
}
