using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemData itemData;
    [SerializeField] Rigidbody2D rb;
    //[SerializeField] Vector2 velocity;

    private void SetupItemVisual()
    {
        if (itemData == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    public void SetupItem(ItemData _itemData, Vector2 velocity)
    {
        itemData = _itemData;
        rb.velocity = velocity;
        SetupItemVisual();
    }
    public void PickupItem(Collider2D collision)//子项中的boxcollider碰撞检测后 调用这个函数
    {
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(itemData, 1);//往仓库添加 1 个这个物品，并删除物品
            Destroy(gameObject);
        }
    }
}
