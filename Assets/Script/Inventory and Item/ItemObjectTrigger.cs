using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    ItemObject _itemObject => GetComponentInParent<ItemObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _itemObject.PickupItem(collision);
    }
}
