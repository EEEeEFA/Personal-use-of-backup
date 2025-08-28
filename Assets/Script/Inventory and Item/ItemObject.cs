using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public void PickupItem(Collider2D collision)//�����е�boxcollider��ײ���� �����������
    {
        if (!Inventory.instance.CanAddItem())
            return;
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(itemData, 1);//���ֿ����� 1 �������Ʒ����ɾ����Ʒ
            Destroy(gameObject);
        }
    }
}
