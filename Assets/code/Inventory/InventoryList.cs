using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
References:
https://www.youtube.com/watch?v=SGz3sbZkfkg
https://learn.unity.com/tutorial/implement-data-persistence-between-scenes
*/

public class InventoryList : MonoBehaviour
{
    public static InventoryList instance;
    private Dictionary<InventoryItemData, InventoryItem> itemDictionary;
    public List<InventoryItem> inventoryList { get; private set; }

    private void Awake()
    {
        Debug.Log("InventoryList Awake called.");
        instance = this;
        DontDestroyOnLoad(gameObject);

        inventoryList = new List<InventoryItem>();
        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    public void AddItem(InventoryItemData _itemData)
    {
        Debug.Log("InventoryList AddItem called.");
        if (itemDictionary.TryGetValue(_itemData, out InventoryItem itemValue)) //If the item exists in the inventory
        {
            Debug.Log("Item exists in inventory. Increasing item stack.");
            itemValue.IncreaseStack();
        } 
        else
        {
            Debug.Log("Item does not exist in inventory. Adding new item.");
            InventoryItem newItem = new InventoryItem(_itemData);
            inventoryList.Add(newItem);
            itemDictionary.Add(_itemData, newItem);
        }
    }

    public void RemoveItem(InventoryItemData _itemData)
    {
        Debug.Log("InventoryList RemoveItem called.");
        if (itemDictionary.TryGetValue(_itemData, out InventoryItem itemValue)) //If the item exists in the inventory
        {
            Debug.Log("Item exists in inventory. Decreasing item stack.");
            itemValue.DecreaseStack();
        }
        if(itemValue.stackSize <= 0)
        {
            Debug.Log("Item stack equal to or less than zero. Removing item from inventory.");
            inventoryList.Remove(itemValue);
            itemDictionary.Remove(_itemData);
        }
    }
}

[Serializable] public class InventoryItem
{
    public InventoryItemData itemData { get; private set; }
    public int stackSize { get; private set; }

    public InventoryItem(InventoryItemData _sourceData)
    {
        itemData = _sourceData;
        IncreaseStack();
    }

    public void IncreaseStack()
    {
        Debug.Log("InventoryItem IncreaseStack called.");
        stackSize++;
    }

    public void DecreaseStack()
    {
        Debug.Log("InventoryItem DecreaseStack called.");
        stackSize--;
    }
}