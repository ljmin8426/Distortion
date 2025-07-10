using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Equipment Item")]
public class EquipmentItemT : ItemSO
{
    public List<ItemStat> statList = new();
}

[System.Serializable]
public class ItemStat
{
    public ITEM_STAT_TYPE statType;
    public float value;
}

public enum ITEM_STAT_TYPE
{
    MaxHP,
    MaxEP,
    Attack,
    MoveSpeed,
    AttackSpeed
}