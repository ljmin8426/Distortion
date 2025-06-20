using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game Data/Player Data")]
public class PlayerData : ScriptableObject
{
    public string characterName;

    [Header("기본 스탯")]
    public float baseMaxHP = 100f;
    public float baseMaxEP = 50f;
    public float baseAttack = 10f;
    public float baseMoveSpeed = 5f;

    [Header("레벨 조건")]
    public int initialExpToLevelUp = 100;
    public float expGrowthRate = 1.2f;

    [Header("레벨 업 보너스")]
    public float hpGrowth = 1.1f;
    public float epGrowth = 1.1f;
    public float atkGrowth = 1.1f;

    [Header("회복량")]
    public float hpRegenRate = 5f; 
    public float epRegenRate = 10f;
}
