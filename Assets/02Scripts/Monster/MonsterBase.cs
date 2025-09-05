using System;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBase : PoolObject, IDamageable
{
    [Header("Monster Id")]
    [SerializeField] private int id;

    [Header("Sound Clip")]
    [SerializeField] private AudioClip hitSoundClip;
    [SerializeField] private AudioClip attackSoundClip;
    [SerializeField] private AudioClip deathSoundClip;

    [Header("Range")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float detectionRange = 10f;

    private bool isDead;

    private float curHP;
    private float maxHP;

    private int animHash_Move = Animator.StringToHash("moveSpeed");

    private Animator animator;

    private NavMeshAgent agent;

    private Transform target;

    private MonsterData monsterData;

    private StateMachine<ENEMY_STATE, MonsterBase> stateMachine;

    private EnemyHPBar hpBar;

    public event Action<MonsterBase> OnMonsterDie;

    public GameObject Owner {  get; private set; }
    public AudioClip AttackSoundClip => attackSoundClip;
    public AudioClip HitSoundClip => hitSoundClip;
    public AudioClip DeathSoundClip => deathSoundClip;
    public Animator Animator => animator;
    public Transform Target => target;
    public MonsterData MonsterData => monsterData;
    public NavMeshAgent Agent => agent;
    public float DetectionRange => detectionRange;
    public float AttackRange => attackRange;
    public bool IsDead => isDead;



    private void Awake()
    {
        Owner = gameObject;
        GameObject playerObj = GameObject.FindWithTag("Player"); 
        if (playerObj != null)
            target = playerObj.transform;
 
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        hpBar = GetComponentInChildren<EnemyHPBar>();
    }

    private void Update()
    {
        stateMachine?.UpdateState();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }

    public void InitStateMachine()
    {
        stateMachine = new StateMachine<ENEMY_STATE, MonsterBase>(
            ENEMY_STATE.Idle,                   
            new MonsterIdleState(this)          
        );

        stateMachine.AddState(ENEMY_STATE.Chase, new MonsterChaseState(this));
        stateMachine.AddState(ENEMY_STATE.Attack, new MonsterAttackState(this));
        stateMachine.AddState(ENEMY_STATE.Hit, new MonsterHitState(this));
        stateMachine.AddState(ENEMY_STATE.Die, new MonsterDieState(this));
    }


    public void InitMonster()
    {
        DataManager.Instance.GetMonsterData(id, out monsterData);

        isDead = false;
        curHP = monsterData.maxHP;
        maxHP = curHP;
        agent.speed = monsterData.moveSpeed;
        agent.stoppingDistance = attackRange;
        gameObject.layer = LayerMask.NameToLayer("Monster");

        if (hpBar != null)
            hpBar.UpdateHPBar(1f);

        InitStateMachine();
    }

    private void UpdateAnimator()
    {
        if (agent != null && animator != null)
        {
            animator.SetFloat(animHash_Move, agent.velocity.magnitude);
        }
    }

    public void TakeDamage(int amount, GameObject attacker)
    {
        if (curHP <= 0) return;

        curHP -= amount;
        curHP = Mathf.Max(curHP, 0);
        hpBar.UpdateHPBar(curHP / maxHP);

        if (curHP <= 0)
        {
            if(isDead) return;

            isDead = true;
            OnMonsterDie?.Invoke(this);
            ChangeState(ENEMY_STATE.Die);
        }
        else
        {
            DamagePopUpGenerator.Instance.CreatePopUp(transform.position, amount.ToString(), Color.red);
            PoolManager.Instance.SpawnFromPool("HitEffect", transform.position, Quaternion.identity);

            ChangeState(ENEMY_STATE.Hit);
        }
    }

    public void ChangeState(ENEMY_STATE nextState)
    {
        stateMachine.ChangeState(nextState);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
