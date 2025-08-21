using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Equipment Item")]
public class EquipmentItem : ItemDataSO
{
    public List<ItemStat> statList = new();

    public ITEM_RARITY rarity;

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
            ITEM_RARITY.Common => 1f,
            ITEM_RARITY.Rare => 1.1f,
            ITEM_RARITY.Epic => 1.25f,
            ITEM_RARITY.Legendary => 1.5f,
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