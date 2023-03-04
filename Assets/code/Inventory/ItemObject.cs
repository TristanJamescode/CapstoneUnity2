using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
References:
https://www.youtube.com/watch?v=SGz3sbZkfkg
*/

public class ItemObject : MonoBehaviour
{
    public InventoryItemData item;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("ItemObject OnTriggerEnter called.");
        InventoryList.instance.AddItem(item);
        Destroy(gameObject);
    }
}
