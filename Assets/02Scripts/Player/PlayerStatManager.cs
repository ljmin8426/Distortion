using System;
using System.Collections;
using UnityEngine;

public class PlayerStatManager : SingletonDestroy<PlayerStatManager>, IDamageable
{
    [Header("Sound")]
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip blockSound;

    [Header("Defaullt Stat")]
    [SerializeField] private PlayerStatSO defaultStat;

    [Header("Exp Table")]
    [SerializeField] private ExpTableSO expTable;

    [Header("Current Stat")]
    [SerializeField] private int level;
    [SerializeField] private float currentHP;
    [SerializeField] private float currentEP;
    [SerializeField] private float currentExp;

    [Header("Base Stat")]
    [SerializeField] private float defaultMaxHP;
    [SerializeField] private float defaultMaxEP;
    [SerializeField] private float defaultAtk;
    [SerializeField] private float defaultAg;

    [Header("Item Stat")]
    [SerializeField] private float bonusMaxHP;
    [SerializeField] private float bonusMaxEP;
    [SerializeField] private float bonusAtk;
    [SerializeField] private float bonusAg;

    [Header("Dash")]
    [SerializeField] private int maxDash = 5;
    [SerializeField] private int dashAmount;
    [SerializeField] private float dashRecoverTime = 2f; // 2초마다 1개 회복

    private bool isDead;

    public int Level => level;
    public float CurrentHP => currentHP;
    public float CurrentEP => currentEP;

    public float MaxHP => defaultMaxHP + bonusMaxHP;
    public float MaxEP => defaultMaxEP + bonusMaxEP;
    public float ATK => defaultAtk + bonusAtk;
    public float AG => defaultAg + bonusAg;

    public float DashAmount => dashAmount;
    public int MaxDash => maxDash;

    private Coroutine dashCoroutine;
    private Coroutine recoverCoroutine;

    public delegate void StatsChange(float value);
    public static event StatsChange OnAtkChange;
    public static event StatsChange OnAgChange;

    public delegate void HpEpChange(float cur, float max);
    public static event HpEpChange OnHpChange;
    public static event HpEpChange OnEpChange;

    public static event Action<int> OnLevelChange;
    public static event Action OnDiePlayer;
    public static event Action<float, float> OnChangeExp;

    public static event Action<int> OnDashChange;

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

        dashAmount = maxDash;
        OnDashChange?.Invoke(dashAmount);

        InvokeAllEvents();
        OnLevelChange?.Invoke(level);
        OnChangeExp?.Invoke(currentExp, expTable.GetExpRequired(level));

        if (recoverCoroutine != null)
            StopCoroutine(recoverCoroutine);
        recoverCoroutine = StartCoroutine(RecoverHPEP());
    }

    public bool UseDash()
    {
        if (dashAmount <= 0) return false;

        dashAmount--;
        OnDashChange?.Invoke(dashAmount);

        if (dashCoroutine == null)
            dashCoroutine = StartCoroutine(DashRecover());

        return true;
    }

    private IEnumerator DashRecover()
    {
        while (dashAmount < maxDash)
        {
            yield return new WaitForSeconds(dashRecoverTime);

            dashAmount++;
            OnDashChange?.Invoke(dashAmount);
        }

        dashCoroutine = null;
    }

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
        OnLevelChange?.Invoke(level);
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        if (isDead) return;

        var shield = GetComponent<Shield>();
        if (shield != null && shield.IsShieldActive())
        {
            AudioManager.Instance.PlaySoundFXClip(blockSound, transform, 1f);
            float remaining = shield.AbsorbDamage(damage);
            if (remaining <= 0f) return;
            damage = remaining;
        }

        currentHP = Mathf.Max(0f, currentHP - damage);
        OnHpChange?.Invoke(currentHP, MaxHP);
        AudioManager.Instance.PlaySoundFXClip(damageSound, transform, 1f);

        if (currentHP <= 0f)
        {
            Die(attacker);
        }
    }

    private void Die(GameObject killer)
    {
        if (isDead) return;
        isDead = true;

        OnDiePlayer?.Invoke();
        // killer 정보 넘길거면: OnDiePlayer?.Invoke(killer);
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
                case ItemStat_Type.MaxHP: bonusMaxHP += stat.value; break;
                case ItemStat_Type.MaxEP: bonusMaxEP += stat.value; break;
                case ItemStat_Type.Attack: bonusAtk += stat.value; break;
                case ItemStat_Type.AttackSpeed: bonusAg += stat.value; break;
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
                case ItemStat_Type.MaxHP: bonusMaxHP -= stat.value; break;
                case ItemStat_Type.MaxEP: bonusMaxEP -= stat.value; break;
                case ItemStat_Type.Attack: bonusAtk -= stat.value; break;
                case ItemStat_Type.AttackSpeed: bonusAg -= stat.value; break;
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
