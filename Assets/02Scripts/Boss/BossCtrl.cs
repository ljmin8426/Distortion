using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class BossCtrl : MonoBehaviour, IDamageable
{
    public enum BossPhase
    {
        Phase1,
        Phase2,
    }

    public enum BossState
    {
        Idle,
        Phase1,
        Phase2,
        Die,
    }

    private BossData bossData;
    private BossPhase curPhase;
    private BossState curState;

    [SerializeField] private int bossId;
    private float curHP;
    private float speed;
    private bool isDead;

    private Transform target;

    private NavMeshAgent agent;
    private Animator animator;

    private StateMachine<BossState, BossCtrl> stateMachine;

    [Header("SoundClip")]
    [SerializeField] private AudioClip phase2Sound;

    public event Action<BossCtrl> OnBossDie;
    public event Action<float, float> OnBossHpChanged;
    public event Action<string> OnBossNameChanged;

    public BossData BossData => bossData;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    public void InitBoss()
    {
        gameObject.layer = LayerMask.NameToLayer("Boss");
        DataManager.Instance.GetBossData(bossId, out bossData);

        curHP = bossData.maxHP;
        speed = bossData.bossSpeed;

        target = GameObject.FindWithTag("Player")?.transform;

        //ChangeState(BossState.Idle);

        isDead = false;

        OnBossNameChanged?.Invoke(bossData.bossName);
        OnBossHpChanged?.Invoke(curHP, bossData.maxHP);
    }

    public void TakeDamage(int amount, GameObject attacker)
    {
        if (curHP <= 0) return;
        if (isDead) return;

        curHP -= amount;
        curHP = Mathf.Max(curHP, 0);
        OnBossHpChanged?.Invoke(curHP, bossData.maxHP);

        if ( curHP > 0 )
        {
            DamagePopUpGenerator.Instance.CreatePopUp(transform.position, amount.ToString(), Color.red);
            PoolManager.Instance.SpawnFromPool("HitEffect", transform.position, Quaternion.identity);
            if(curPhase == BossPhase.Phase1 && curHP <= bossData.maxHP / 2)
            {
                ChangeState(BossState.Phase2);
            }
        }
        else
        {
            // Death
            isDead = true;
            OnBossDie?.Invoke(this);
        }
    }

    public void ChangeState(BossState nextState)
    {
        stateMachine.ChangeState(nextState);
    }
}
