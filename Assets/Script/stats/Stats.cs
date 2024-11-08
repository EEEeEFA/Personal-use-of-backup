using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]//好像作用是让传到别的脚本的参数，在别的脚本的inspetor中显示出来
public class Stats
{
    [SerializeField] private int baseValue;
    public List<int> modifiers;
    public int GetVuale()
    {
        //baseValue + modifiers → final
        return 0;
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
