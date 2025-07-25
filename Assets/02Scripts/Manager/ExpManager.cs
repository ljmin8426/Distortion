using System;
using UnityEngine;

public class ExpManager : Singleton<ExpManager>
{
    private float currentExp;
    private float expToNextLevel;

    public float CurrentExp => currentExp;
    public float ExpToNextLevel => expToNextLevel;

    public static event Action<float, float> OnChangeExp;

    private void Start()
    {
        InitializeExp();
    }

    private void InitializeExp()
    {
        currentExp = 0f;
        expToNextLevel = GameManager.instance.playerData.initialExpToLevelUp;
        OnChangeExp?.Invoke(currentExp, expToNextLevel);
    }

    public void GetExp(float amount)
    {
        currentExp += amount;
        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }

        OnChangeExp?.Invoke(currentExp, expToNextLevel);
    }

    private void LevelUp()
    {
        expToNextLevel *= GameManager.instance.playerData.expGrowthRate;

        PlayerStatManager.instance.ApplyLevelUp();

        Debug.Log("레벨 업! 현재 레벨: " + PlayerStatManager.instance.Level);
    }
}
