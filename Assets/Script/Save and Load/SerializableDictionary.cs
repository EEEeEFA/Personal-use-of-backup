using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();


    //序列化前处理 (OnBeforeSerialize)
    //    触发时机：当 Unity 序列化对象时（如保存场景或进入播放模式）。
    //作用：
    //清空现有的 keys 和 values 列表。
    //遍历字典中的所有键值对，将键存入 keys 列表，值存入 values 列表。
    //结果：字典中的数据被转换为两个可序列化的列表。
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> kvp in this)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    //    反序列化后处理(OnAfterDeserialize)
    //触发时机：当 Unity 反序列化对象时（如加载场景或从存档中还原数据）。
    //作用：
    //清空字典，确保它是空的。
    //检查 keys 和 values 列表的长度是否一致。如果不一致，打印错误信息。
    //遍历 keys 和 values，将键值对逐一添加到字典中。
    //结果：列表中的数据被还原到字典中。

    public void OnAfterDeserialize()
    {
        this.Clear();


        if (keys.Count != values.Count)
        {
            Debug.Log("键数和值数不相等");//
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }

}



