using System;
using System.Collections;
using UnityEngine;

public class PlayerStatManager : SingletonDestroy<PlayerStatManager>
{
    public delegate void StatsChange(float value);
    public static event StatsChange OnAttackPowerChange;
    public static event StatsChange OnMoveSpeedChange;

    public delegate void HpEpChange(float value1, float value2);
    public static event HpEpChange OnHpChange;
    public static event HpEpChange OnEpChange;

    public static event Action<int> OnLevelChange;
    public static event Action OnDiePlayer;


    // ±âº» ½ºÅÈ
    [Header("Base Stat")]
    [SerializeField] private float baseMaxHP;
    [SerializeField] private float baseMaxEP;
    [SerializeField] private float baseAttackPower;
    [SerializeField] private float baseAttackSpeed;
    [SerializeField] private float baseMoveSpeed;

    // Àåºñ ½ºÅÈ
    [Header("Item Stat")]
    [SerializeField] private float bonusMaxHP;
    [SerializeField] private float bonusMaxEP;
    [SerializeField] private float bonusAttackPower;
    [SerializeField] private float bonusMoveSpeed;
    [SerializeField] private float bonusAttackSpeed;

    // ÇöÀç »óÅÂ
    [Header("Current Stat")]
    [SerializeField] private int level;
    [SerializeField] private float currentHP;
    [SerializeField] private float currentEP;   

    public int Level => level;
    public float MaxHP => baseMaxHP + bonusMaxHP;
    public float MaxEP => baseMaxEP + bonusMaxEP;
    public float CurrentHP => currentHP;
    public float CurrentEP => currentEP;
    public float AttackPower => baseAttackPower + bonusAttackPower;
    public float MoveSpeed => baseMoveSpeed + bonusMoveSpeed;
    public float AttackSpeed => baseAttackSpeed + bonusAttackSpeed;

    private Coroutine recoverCoroutine;

    private void Start()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        level = 1;

        var data = GameManager.Instance.playerData;
        //baseMaxHP = data.baseMaxHP;
        //baseMaxEP = data.baseMaxEP;
        //baseAttackPower = data.baseAttack;
        //baseMoveSpeed = data.baseMoveSpeed;
        //baseAttackSpeed = data.baseAttackSpeed;

        currentHP = MaxHP;
        currentEP = MaxEP;

        InvokeAllEvents();
        OnLevelChange?.Invoke(level);

        if (recoverCoroutine != null)
            StopCoroutine(recoverCoroutine);
        recoverCoroutine = StartCoroutine(RecoverHPEP());
    }

    private IEnumerator RecoverHPEP()
    {
        float interval = 0.1f;
        WaitForSeconds wait = new WaitForSeconds(interval);

        var data = GameManager.Instance.playerData;

        //while (true)
        //{
        //    if (currentHP < MaxHP)
        //    {
        //        currentHP = Mathf.Min(currentHP + data.hpRegenRate * interval, MaxHP);
        //        OnHpChange?.Invoke(currentHP, MaxHP);
        //    }

        //    if (currentEP < MaxEP)
        //    {
        //        currentEP = Mathf.Min(currentEP + data.epRegenRate * interval, MaxEP);
        //        OnEpChange?.Invoke(currentEP, MaxEP);
        //    }

        //    yield return wait;
        //}

        yield return null;
    }

    public void ApplyLevelUp()
    {
        level++;
        var data = GameManager.Instance.playerData;

        currentHP = MaxHP;
        currentEP = MaxEP;

        InvokeAllEvents();
        OnLevelChange?.Invoke(level);
    }

    public void TakeDamage(float amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);
        OnHpChange?.Invoke(currentHP, MaxHP);

        if (currentHP <= 0f)
        {
            Debug.Log("<color=red>[Player] »ç¸Á</color>");
            OnDiePlayer?.Invoke();
        }
    }

    public void ConsumeEP(float amount)
    {
        currentEP = Mathf.Max(0, currentEP - amount);
        OnEpChange?.Invoke(currentEP, MaxEP);
    }

    public void RecoverHP(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, MaxHP);
        OnHpChange?.Invoke(currentHP, MaxHP);
    }

    public void RecoverEP(float amount)
    {
        currentEP = Mathf.Min(currentEP + amount, MaxEP);
        OnEpChange?.Invoke(currentEP, MaxEP);
    }

    public void Equip(EquipmentItem equipment)
    {
        foreach (var stat in equipment.GetModifiedStats())
        {
            switch (stat.statType)
            {
                case ITEM_STAT_TYPE.MaxHP:
                    bonusMaxHP += stat.value;
                    break;
                case ITEM_STAT_TYPE.MaxEP:
                    bonusMaxEP += stat.value;
                    break;
                case ITEM_STAT_TYPE.Attack:
                    bonusAttackPower += stat.value;
                    break;
                case ITEM_STAT_TYPE.MoveSpeed:
                    bonusMoveSpeed += stat.value;
                    break;
                case ITEM_STAT_TYPE.AttackSpeed:
                    bonusAttackSpeed += stat.value;
                    break;
            }
        }

        ClampCurrentStat();
        InvokeAllEvents();
    }

    public void UnEquip(EquipmentItem equipment)
    {
        foreach (var stat in equipment.GetModifiedStats())
        {
            switch (stat.statType)
            {
                case ITEM_STAT_TYPE.MaxHP:
                    bonusMaxHP -= stat.value;
                    break;
                case ITEM_STAT_TYPE.MaxEP:
                    bonusMaxEP -= stat.value;
                    break;
                case ITEM_STAT_TYPE.Attack:
                    bonusAttackPower -= stat.value;
                    break;
                case ITEM_STAT_TYPE.MoveSpeed:
                    bonusMoveSpeed -= stat.value;
                    break;
                case ITEM_STAT_TYPE.AttackSpeed:
                    bonusAttackSpeed -= stat.value;
                    break;
            }
        }

        ClampCurrentStat();
        InvokeAllEvents();
    }

    private void ClampCurrentStat()
    {
        currentHP = Mathf.Min(currentHP, MaxHP);
        currentEP = Mathf.Min(currentEP, MaxEP);
    }

    private void InvokeAllEvents()
    {
        OnHpChange?.Invoke(currentHP, MaxHP);
        OnEpChange?.Invoke(currentEP, MaxEP);
        OnAttackPowerChange?.Invoke(AttackPower);
        OnMoveSpeedChange?.Invoke(MoveSpeed);
    }
}
