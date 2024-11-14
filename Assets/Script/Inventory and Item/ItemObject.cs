using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemData itemData;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Vector2 velocity;

    private void OnValidate()
    {
        if (itemData == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    public void SetupItem(ItemData itemData, Vector2 velocity)
    {
        this.itemData = itemData;
        this.velocity = velocity;   
    }
    public void PickupItem(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
