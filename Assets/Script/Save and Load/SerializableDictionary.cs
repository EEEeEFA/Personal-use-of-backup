using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();


    //���л�ǰ���� (OnBeforeSerialize)
    //    ����ʱ������ Unity ���л�����ʱ���籣�泡������벥��ģʽ����
    //���ã�
    //������е� keys �� values �б�
    //�����ֵ��е����м�ֵ�ԣ��������� keys �б�ֵ���� values �б�
    //������ֵ��е����ݱ�ת��Ϊ���������л����б�
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

    //    �����л�����(OnAfterDeserialize)
    //����ʱ������ Unity �����л�����ʱ������س�����Ӵ浵�л�ԭ���ݣ���
    //���ã�
    //����ֵ䣬ȷ�����ǿյġ�
    //��� keys �� values �б�ĳ����Ƿ�һ�¡������һ�£���ӡ������Ϣ��
    //���� keys �� values������ֵ����һ��ӵ��ֵ��С�
    //������б��е����ݱ���ԭ���ֵ��С�

    public void OnAfterDeserialize()
    {
        this.Clear();


        if (keys.Count != values.Count)
        {
            Debug.Log("������ֵ�������");//
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }

}



