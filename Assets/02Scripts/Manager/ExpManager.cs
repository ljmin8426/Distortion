using System;
using UnityEngine;

public class ExpManager : SingletonDestroy<ExpManager>
{
    private float currentExp;
    private float expToNextLevel;

    public static event Action<float, float> OnChangeExp;

    private void Start()
    {
        InitializeExp();
    }

    private void InitializeExp()
    {
        currentExp = 0f;
        expToNextLevel = 10;
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
        expToNextLevel *= 1111;

        PlayerStatManager.Instance.ApplyLevelUp();

        Debug.Log("레벨 업! 현재 레벨: " + PlayerStatManager.Instance.Level);
    }
}
