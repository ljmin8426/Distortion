using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class BossCtrl : MonoBehaviour, IDamageable
{
    public enum BossState
    {
        Idle,
        Phase1,
        Phase2,
        Die,
    }

    [SerializeField] private BossId bossId;

    [Header("SoundClip")]
    [SerializeField] private AudioClip phase2Sound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Phase1")]
    [SerializeField] private float detectionRange;

    private float curHP;
    private bool isDead;

    private Transform target;

    private NavMeshAgent agent;
    private Animator animator;

    private StateMachine<BossState, BossCtrl> stateMachine;

    private BossData bossData;
    private BossState curState;

    public event Action<BossCtrl> OnBossDie;
    public event Action<BossCtrl> OnFightReady;
    public event Action<float, float> OnBossHpChanged;
     
    public BossData BossData => bossData;
    public NavMeshAgent Agent => agent;
    public Transform Target => target;
    public Animator Animator => animator;

    public float DectectionRange => detectionRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        stateMachine = new StateMachine<BossState, BossCtrl>(BossState.Idle, new BossIdleState(this));
        stateMachine.AddState(BossState.Phase1, new BossPhase1State(this));

        InitBoss();
    }

    private void Update()
    {
        stateMachine.UpdateState();
    }
    
    private void FixedUpdate()
    {
        stateMachine.FixedUpdateState();
    }
    public void InitBoss()
    {
        gameObject.layer = LayerMask.NameToLayer("Boss");
        DataManager.Instance.GetBossData((int)bossId, out bossData);

        curHP = bossData.maxHP;
        target = GameObject.FindWithTag("Player")?.transform;

        isDead = false;
        OnBossHpChanged?.Invoke(curHP, bossData.maxHP);

        ChangeState(BossState.Idle);
    }


    public void TakeDamage(float amount, GameObject attacker)
    {
        if (curHP <= 0) return;
        if (isDead) return;

        curHP -= amount;
        curHP = Mathf.Max(curHP, 0);
        OnBossHpChanged?.Invoke(curHP, bossData.maxHP);

        DamagePopUpGenerator.Instance.CreatePopUp(transform.position, amount.ToString(), Color.red);
        PoolManager.Instance.SpawnFromPool("HitEffect", transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySoundFXClip(hitSound, transform, 1f);

        if ( curHP > 0 )
        {
            if(curState == BossState.Phase1 && curHP <= bossData.maxHP / 2)
            {
                ChangeState(BossState.Phase2);
            }
        }
        else
        {
            isDead = true;
            OnBossDie?.Invoke(this);
        }
    }

    public void StartFight()
    {
        ChangeState(BossState.Phase1);
        OnFightReady?.Invoke(this);
    }

    public void ChangeState(BossState nextState)
    {
        stateMachine.ChangeState(nextState);
    }
}
