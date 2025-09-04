using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int itemId;
    public string itemName;
    public string itemDescription;
    public Sprite itemiconImg;
    public int sellGold;
    public bool equip;
}

[System.Serializable]
public class WeaponItemData : ItemData
{
    public int attackDamage;
}

[System.Serializable]
public class MonsterData
{
    public int monsterId;
    public string monsterName;
    public string monsterDescription;
    public int moveSpeed;
    public int attackSpeed;
    public int attackDamage;
    public int maxHP;
    public GameObject[] dropItem;
    public int dropRate;
    public int EXP;
}

[System.Serializable]
public class EXPTable
{
    public int curLevel;
    public int nextEXP;
}



