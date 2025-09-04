using System;
using System.Collections;
using UnityEngine;

public class PlayerStatManager : SingletonDestroy<PlayerStatManager>, IDamageable
{
    // 이벤트
    public delegate void StatsChange(float value);
    public static event StatsChange OnAtkChange;
    public static event StatsChange OnAgChange;

    public delegate void HpEpChange(float cur, float max);
    public static event HpEpChange OnHpChange;
    public static event HpEpChange OnEpChange;

    public static event Action<int> OnLevelChange;
    public static event Action OnDiePlayer;
    public static event Action<float, float> OnChangeExp;

    [Header("Defaullt Stat")]
    [SerializeField] private PlayerStatSO defaultStat;

    [Header("Exp Table")]
    [SerializeField] private ExpTableSO expTable;

    // 현재 상태
    [Header("Current Stat")]
    [SerializeField] private int level;
    [SerializeField] private float currentHP;
    [SerializeField] private float currentEP;
    [SerializeField] private float currentExp;

    // 기본 스탯
    [Header("Base Stat")]
    [SerializeField] private float defaultMaxHP;
    [SerializeField] private float defaultMaxEP;
    [SerializeField] private float defaultAtk;
    [SerializeField] private float defaultAg;

    // 장비 스탯
    [Header("Item Stat")]
    [SerializeField] private float bonusMaxHP;
    [SerializeField] private float bonusMaxEP;
    [SerializeField] private float bonusAtk;
    [SerializeField] private float bonusAg;

    [Header("Sound")]
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip levelUpSound;

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
        currentExp = 0f;

        defaultMaxHP = defaultStat.hp;
        defaultMaxEP = defaultStat.ep;
        defaultAtk = defaultStat.atk;
        defaultAg = defaultStat.ag;

        currentHP = MaxHP;
        currentEP = MaxEP;

        InvokeAllEvents();
        OnLevelChange?.Invoke(level);
        OnChangeExp?.Invoke(currentExp, expTable.GetExpRequired(level));

        if (recoverCoroutine != null)
            StopCoroutine(recoverCoroutine);
        recoverCoroutine = StartCoroutine(RecoverHPEP());
    }

    // 경험치 획득
    public void GetExp(float amount)
    {
        currentExp += amount;
        while (currentExp >= expTable.GetExpRequired(level))
        {
            currentExp -= expTable.GetExpRequired(level);
            LevelUp();
        }
        OnChangeExp?.Invoke(currentExp, expTable.GetExpRequired(level));
    }


    private void LevelUp()
    {
        level++;
        currentHP = MaxHP;
        currentEP = MaxEP;

        InvokeAllEvents();
        AudioManager.Instance.PlaySoundFXClip(levelUpSound, transform, 1f);
        OnLevelChange?.Invoke(level);
    }
    // 피해 처리
    public void TakeDamage(int amount, GameObject attacker)
    {
        var shield = GetComponent<Shield>();
        if (shield != null && shield.IsShieldActive())
        {
            int remaining = shield.AbsorbDamage(amount);
            if (remaining <= 0) return;
            amount = remaining;
        }
        TakeDamage((float)amount);
    }

    public void TakeDamage(float amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);
        OnHpChange?.Invoke(currentHP, MaxHP);
        AudioManager.Instance.PlaySoundFXClip(damageSound, transform, 1f);

        if (currentHP <= 0f)
        {
            Debug.Log("<color=red>[Player] 사망</color>");
            OnDiePlayer?.Invoke();
        }
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

    // EP, HP 소비/회복
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

    // 장비 장착/해제
    public void Equip(EquipmentItem equipment)
    {
        foreach (var stat in equipment.GetModifiedStats())
        {
            switch (stat.statType)
            {
                case ITEM_STAT_TYPE.MaxHP: bonusMaxHP += stat.value; break;
                case ITEM_STAT_TYPE.MaxEP: bonusMaxEP += stat.value; break;
                case ITEM_STAT_TYPE.Attack: bonusAtk += stat.value; break;
                case ITEM_STAT_TYPE.AttackSpeed: bonusAg += stat.value; break;
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
                case ITEM_STAT_TYPE.MaxHP: bonusMaxHP -= stat.value; break;
                case ITEM_STAT_TYPE.MaxEP: bonusMaxEP -= stat.value; break;
                case ITEM_STAT_TYPE.Attack: bonusAtk -= stat.value; break;
                case ITEM_STAT_TYPE.AttackSpeed: bonusAg -= stat.value; break;
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
        OnAtkChange?.Invoke(ATK);
        OnAgChange?.Invoke(AG);
    }
}
