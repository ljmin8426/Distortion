using UnityEngine;

public class ExpManager : Singleton<ExpManager>
{
    [SerializeField] private PlayerData playerData;

    private float currentExp;
    public float CurrentExp => currentExp;

    private float expToNextLevel;
    public float ExpToNextLevel => expToNextLevel;

    private void Start()
    {
        InitializeExp();
    }

    private void InitializeExp()
    {
        currentExp = 0f;
        expToNextLevel = playerData.initialExpToLevelUp;
    }

    public void GainExp(float amount)
    {
        currentExp += amount;
        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        expToNextLevel *= playerData.expGrowthRate;

        PlayerStatManager.instance.ApplyLevelUpGrowth();

        Debug.Log("레벨 업! 현재 레벨: " + PlayerStatManager.instance.Level);
    }
}
