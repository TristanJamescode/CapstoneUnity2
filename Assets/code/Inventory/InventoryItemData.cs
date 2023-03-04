using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
References:
https://www.youtube.com/watch?v=SGz3sbZkfkg
*/

[CreateAssetMenu(menuName = "Item Data")]
public class InventoryItemData : ScriptableObject
{
    public string id;
    public string displayName;
    //public Sprite icon;
    public GameObject prefab;
}
