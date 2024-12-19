using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfDropItems;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()//��possibleDrop���ж������Ƿ���䣬�����������dropList
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
                dropList.Add(possibleDrop[i]);
        }

        for (int i = 0; i < amountOfDropItems; i++)//��dropList�г� amountOfDropItems ������
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
            dropList.Remove(randomItem);
            DropItem(randomItem);//����������Ϣ����ʵ��������,����������ٶȵ���ʵ��
        }
    }
    public void DropItem(ItemData randomItem)//���ɵ�����    
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));
        newDrop.GetComponent<ItemObject>().SetupItem(randomItem, randomVelocity);
    }
}
