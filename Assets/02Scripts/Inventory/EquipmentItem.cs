using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStat
{
    public ItemStat_Type statType;
    public int value;

    public ItemStat(ItemStat_Type type, int value)
    {
        this.statType = type;
        this.value = value;
    }
}

public enum ItemStat_Type
{
    MaxHP,
    MaxEP,
    Attack,
    MoveSpeed,
    AttackSpeed
}

[CreateAssetMenu(menuName = "Inventory/Equipment Item")]
public class EquipmentItem : ItemDataSO
{
    public List<ItemStat> statList = new();

    public Item_Rarity rarity;

    public List<ItemStat> GetModifiedStats()
    {
        float multiplier = GetRarityMultiplier();

        List<ItemStat> modified = new();
        foreach (var stat in statList)
        {
            int modifiedValue = Mathf.RoundToInt(stat.value * multiplier);
            modified.Add(new ItemStat(stat.statType, modifiedValue));
        }

        return modified;
    }

    private float GetRarityMultiplier()
    {
        return rarity switch
        {
            Item_Rarity.Common => 1f,
            Item_Rarity.Rare => 1.1f,
            Item_Rarity.Epic => 1.25f,
            Item_Rarity.Legendary => 1.5f,
            _ => 1f
        };
    }
}