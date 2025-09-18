using System;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBase : PoolObject, IDamageable
{
    public enum Monster_Id
    {
        None = 0,
        Melee = 1000,
        Polearm = 2000,
        Hammer = 3000
    }

    [Header("Monster Id")]
    [SerializeField] private Monster_Id id;

    [Header("Sound Clip")]
    [SerializeField] private AudioClip hitSoundClip;
    [SerializeField] private AudioClip attackSoundClip;
    [SerializeField] private AudioClip deathSoundClip;

    [Header("Range")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float detectionRange = 10f;

    [Header("HitState")]
    [SerializeField] private float stunTime = 1;

    private bool isDead;

    private float curHP;
    private float maxHP;

    private int animHash_Move = Animator.StringToHash("moveSpeed");

    private Animator animator;

    private NavMeshAgent agent;

    private Transform target;

    private MonsterData monsterData;

    private StateMachine<Enemy_State, MonsterBase> stateMachine;

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
    public float StunTime => stunTime;
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
        stateMachine = new StateMachine<Enemy_State, MonsterBase>(
            Enemy_State.Idle,                   
            new MonsterIdleState(this)          
        );

        stateMachine.AddState(Enemy_State.Chase, new MonsterChaseState(this));
        stateMachine.AddState(Enemy_State.Attack, new MonsterAttackState(this));
        stateMachine.AddState(Enemy_State.Hit, new MonsterHitState(this));
        stateMachine.AddState(Enemy_State.Die, new MonsterDieState(this));
    }

    public void InitMonster()
    {
        DataManager.Instance.GetMonsterData((int)id, out monsterData);

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

    public void TakeDamage(float amount, GameObject attacker)
    {
        if (curHP <= 0) return;

        curHP -= amount;
        curHP = Mathf.Max(curHP, 0);
        hpBar.UpdateHPBar(curHP / maxHP);

        DamagePopUpGenerator.Instance.CreatePopUp(transform.position, amount.ToString(), Color.red);
        PoolManager.Instance.SpawnFromPool("HitEffect", transform.position, Quaternion.identity);

        if (curHP <= 0)
        {
            if(isDead) return;

            isDead = true;
            OnMonsterDie?.Invoke(this);
            ChangeState(Enemy_State.Die);
        }
        else
        {
            ChangeState(Enemy_State.Hit);
        }
    }

    public void ChangeState(Enemy_State nextState)
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
