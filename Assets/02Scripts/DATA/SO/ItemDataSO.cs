using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public Item_Type itemType;
    public int maxStack;
}