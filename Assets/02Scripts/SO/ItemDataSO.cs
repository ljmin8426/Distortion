using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public ITEM_TYPE itemType;
    public int maxStack;
}