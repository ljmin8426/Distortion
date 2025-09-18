using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Equipment Item")]
public class EquipmentItem : ItemDataSO
{
    public List<ItemStat> statList = new();

    public Item_Rarity rarity;

    /// <summary>
    /// 등급 보정이 반영된 스탯 리스트 반환
    /// </summary>
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

[System.Serializable]
public class ItemStat
{
    public ITEM_STAT_TYPE statType;
    public int value;

    public ItemStat(ITEM_STAT_TYPE type, int value)
    {
        this.statType = type;
        this.value = value;
    }
}

public enum ITEM_STAT_TYPE
{
    MaxHP,
    MaxEP,
    Attack,
    MoveSpeed,
    AttackSpeed
}