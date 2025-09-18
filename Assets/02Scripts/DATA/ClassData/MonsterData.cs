using UnityEngine;

[System.Serializable]
public class MonsterData
{
    [Header("Info")]
    public string monsterName;
    public int monsterId;

    [Header("Stat")]
    public int maxHP;
    public int moveSpeed;
    public int attackSpeed;
    public int attackDamage;

    [Header("Reward")]
    public int EXP;
}

[System.Serializable]
public class BossData
{
    [Header("Info")]
    public string bossName;
    public int bossId;

    [Header("Stat")]
    public int bossSpeed;
    public int maxHP;
}



