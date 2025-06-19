using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game Data/Player Data")]
public class PlayerData : ScriptableObject
{
    public string characterName;

    [Header("Base Stats")]
    public float baseMaxHP = 100f;
    public float baseMaxEP = 50f;
    public float baseAttack = 10f;
    public float baseMoveSpeed = 5f;

    [Header("Leveling")]
    public float initialExpToLevelUp = 100f;
    public float expGrowthRate = 1.2f;

    [Header("Level Up Bonuses (multipliers)")]
    public float hpGrowth = 1.1f;
    public float epGrowth = 1.1f;
    public float atkGrowth = 1.1f;

    [Header("Regeneration")]
    public float hpRegenRate = 5f; // 초당 HP 회복량
    public float epRegenRate = 10f; // 초당 EP 회복량
}
