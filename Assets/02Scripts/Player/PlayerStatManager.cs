using System;
using System.Collections;
using UnityEngine;

public class PlayerStatManager : SingletonDestroy<PlayerStatManager>, IDamageable
{
    public delegate void StatsChange(float value);
    public static event StatsChange OnAttackPowerChange;

    public delegate void HpEpChange(float curEP, float maxEP);
    public static event HpEpChange OnHpChange;
    public static event HpEpChange OnEpChange;

    public static event Action<int> OnLevelChange;
    public static event Action OnDiePlayer;

    [SerializeField] private PlayerStatSO defaultStat;

    // ÇöÀç »óÅÂ
    [Header("Current Stat")]
    [SerializeField] private int level;
    [SerializeField] private float currentHP;
    [SerializeField] private float currentEP;

    // ±âº» ½ºÅÈ
    [Header("Base Stat")]
    [SerializeField] private float defaultMaxHP;
    [SerializeField] private float defaultMaxEP;
    [SerializeField] private float defaultAtk;
    [SerializeField] private float defaultAg;

    // Àåºñ ½ºÅÈ
    [Header("Item Stat")]
    [SerializeField] private float bonusMaxHP;
    [SerializeField] private float bonusMaxEP;
    [SerializeField] private float bonusAtk;
    [SerializeField] private float bonusAg;


    public int Level => level;
    public float CurrentHP => currentHP;
    public float CurrentEP => currentEP;

    public float MaxHP => defaultMaxHP + bonusMaxHP;
    public float MaxEP => defaultMaxEP + bonusMaxEP;
    public float ATK => defaultAtk + bonusAtk;
    public float AG => defaultAg + bonusAg;

    private Coroutine recoverCoroutine;

    private void Start()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        level = 1;

        defaultMaxHP = defaultStat.hp; 
        defaultMaxEP = defaultStat.ep;
        defaultAtk = defaultStat.atk;
        defaultAg = defaultStat.ag;



        currentHP = MaxHP;
        currentEP = MaxEP;

        InvokeAllEvents();
        OnLevelChange?.Invoke(level);

        if (recoverCoroutine != null)
            StopCoroutine(recoverCoroutine);
        recoverCoroutine = StartCoroutine(RecoverHPEP());
    }

    public void TakeDamage(int amount, GameObject attacker)
    {
        var shield = GetComponent<Shield>();
        if (shield != null && shield.IsShieldActive())
        {
            int remaining = shield.AbsorbDamage(amount);
            if (remaining <= 0)
            {
                return;
            }

            amount = remaining;
        }

        PlayerStatManager.Instance.TakeDamage(amount);
    }
    private IEnumerator RecoverHPEP()
    {
        float interval = 1f;
        WaitForSeconds wait = new WaitForSeconds(interval);

        while (true)
        {
            if (currentHP < MaxHP)
            {
                currentHP = Mathf.Min(currentHP + 1 * interval, MaxHP);
                OnHpChange?.Invoke(currentHP, MaxHP);
            }

            if (currentEP < MaxEP)
            {
                currentEP = Mathf.Min(currentEP + 1 * interval, MaxEP);
                OnEpChange?.Invoke(currentEP, MaxEP);
            }

            yield return wait;
        }
    }

    public void ApplyLevelUp()
    {
        level++;

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
                    bonusAtk += stat.value;
                    break;
                case ITEM_STAT_TYPE.AttackSpeed:
                    bonusAg += stat.value;
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
                    bonusAtk -= stat.value;
                    break;
                case ITEM_STAT_TYPE.AttackSpeed:
                    bonusAg -= stat.value;
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
        OnAttackPowerChange?.Invoke(ATK);
    }
}
