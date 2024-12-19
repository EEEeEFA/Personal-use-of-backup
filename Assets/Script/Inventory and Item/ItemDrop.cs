using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfDropItems;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()//在possibleDrop中判定物体是否掉落，若掉落则加入dropList
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
                dropList.Add(possibleDrop[i]);
        }

        for (int i = 0; i < amountOfDropItems; i++)//在dropList中抽 amountOfDropItems 个掉落
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
            dropList.Remove(randomItem);
            DropItem(randomItem);//将掉落物信息传给实例化对象,并随机生成速度弹出实例
        }
    }
    public void DropItem(ItemData randomItem)//生成凋落物    
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));
        newDrop.GetComponent<ItemObject>().SetupItem(randomItem, randomVelocity);
    }
}
