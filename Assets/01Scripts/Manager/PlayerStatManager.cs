using System;
using System.Collections;
using UnityEngine;

public class PlayerStatManager : Singleton<PlayerStatManager>
{
    public delegate void StatsChange(float value);
    public static event StatsChange OnAttackPowerChange;
    public static event StatsChange OnMoveSpeedChange;

    public delegate void HpEpChange(float value1, float value2);
    public static event HpEpChange OnHpChange;
    public static event HpEpChange OnEpChange;

    public static event Action<int> OnLevelChange;
    public static event Action OnDiePlayer;

    private int level;

    // Base stats
    [SerializeField] private float maxHP;
    [SerializeField] private float maxEP;
    [SerializeField] private float attackPower;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackSpeed;

    private float currentHP;
    private float currentEP;

    public int Level => level;
    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public float MaxEP => maxEP;
    public float CurrentEP => currentEP;
    public float AttackPower => attackPower;
    public float MoveSpeed => moveSpeed;
    public float AttackSpeed => attackSpeed;    

    private Coroutine recoverCoroutine;

    private void Start()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        level = 1;

        maxHP = GameManager.instance.playerData.baseMaxHP;
        maxEP = GameManager.instance.playerData.baseMaxEP;
        attackPower = GameManager.instance.playerData.baseAttack;
        moveSpeed = GameManager.instance.playerData.baseMoveSpeed;

        currentHP = MaxHP;
        currentEP = MaxEP;

        OnLevelChange?.Invoke(level);
        OnHpChange?.Invoke(currentHP, MaxHP);
        OnEpChange?.Invoke(currentEP, MaxEP);
        OnAttackPowerChange?.Invoke(AttackPower);
        OnMoveSpeedChange?.Invoke(moveSpeed);

        if (recoverCoroutine != null)
            StopCoroutine(recoverCoroutine);
        recoverCoroutine = StartCoroutine(RecoverHPEP());
    }

    private IEnumerator RecoverHPEP()
    {
        float interval = 0.1f;
        WaitForSeconds wait = new WaitForSeconds(interval);

        while (true)
        {
            if (currentHP < MaxHP)
            {
                currentHP = Mathf.Min(currentHP + GameManager.instance.playerData.hpRegenRate * interval, MaxHP);
                OnHpChange?.Invoke(currentHP, MaxHP);
            }

            if (currentEP < MaxEP)
            {
                currentEP = Mathf.Min(currentEP + GameManager.instance.playerData.epRegenRate * interval, MaxEP);
                OnEpChange?.Invoke(currentEP, MaxEP);
            }

            yield return wait;
        }
    }

    public void ApplyLevelUp()
    {
        level++;
        maxHP *= GameManager.instance.playerData.hpGrowth;
        maxEP *= GameManager.instance.playerData.epGrowth;
        attackPower *= GameManager.instance.playerData.atkGrowth;
        currentHP = MaxHP;
        currentEP = MaxEP;

        OnLevelChange?.Invoke(level);
        OnHpChange?.Invoke(currentHP, MaxHP);
        OnEpChange?.Invoke(currentEP, MaxEP);
        OnAttackPowerChange?.Invoke(AttackPower);
        OnMoveSpeedChange?.Invoke(moveSpeed);
    }

    public void TakeDamage(float amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);

        if(currentHP < 1)
        {
            currentHP = 0;
            OnDiePlayer?.Invoke();
        }

        OnHpChange?.Invoke(currentHP, MaxHP);
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

    // 장비/아이템 관련 메서드
    public void AddStatModifier(float hp = 0, float ep = 0, float atk = 0, float move = 0)
    {
        maxHP += hp;
        maxEP += ep;
        attackPower += atk;
        moveSpeed += move;

        OnHpChange?.Invoke(currentHP, MaxHP);
        OnEpChange?.Invoke(currentEP, MaxEP);
        OnAttackPowerChange?.Invoke(AttackPower);
        OnMoveSpeedChange?.Invoke(moveSpeed);
    }

    public void RemoveStatModifier(float hp = 0, float ep = 0, float atk = 0, float move = 0)
    {
        maxHP -= hp;
        maxEP -= ep;
        attackPower -= atk;
        moveSpeed -= move;

        OnHpChange?.Invoke(currentHP, MaxHP);
        OnEpChange?.Invoke(currentEP, MaxEP);
        OnAttackPowerChange?.Invoke(AttackPower);
        OnMoveSpeedChange?.Invoke(moveSpeed);
    }
}
