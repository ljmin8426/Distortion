using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : MonoBehaviour, IDamageable
{                                                                 
    [Header("Enemy Data")]                                        
    [SerializeField] private EnemyDateSO enemyData;               
                                                                  
    [Header("Sound Clip")]                                        
    [SerializeField] private AudioClip damageSoundClip;           
    [SerializeField] private AudioClip attackSoundClip;           
                                                                  
    protected StateMachine<ENEMY_STATE, EnemyBase> stateMachine;  
                                                                  
    private EnemyHPBar hpBar;                                     
                                                                  
    public EnemyDateSO EnemyData => enemyData;
    public EnemyAttackCollider AttackCollider { get; private set; }
    public Transform Player { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Animator Animator { get; private set; }
    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }

    protected virtual void Awake()
    {
        if (enemyData != null)
        {
            MaxHP = enemyData.maxHP;
            CurrentHP = MaxHP;

            if (hpBar != null)
            {
                hpBar.UpdateHPBar(1f);
            }
        }

        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponentInChildren<Animator>();
        AttackCollider = GetComponentInChildren<EnemyAttackCollider>();
        hpBar = GetComponentInChildren<EnemyHPBar>();

        Agent.stoppingDistance = enemyData.attackRange;
        Agent.speed = enemyData.moveSpeed;

        InitStateMachine();
    }

    protected virtual void Update()
    {
        stateMachine?.UpdateState();
        UpdateAnimator();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }

    protected void UpdateAnimator()
    {
        float speed = Agent.velocity.magnitude;
        Animator.SetFloat("Movespeed", speed);
    }

    public virtual void TakeDamage(int amount, GameObject attacker)
    {
        if (CurrentHP <= 0) return;

        CurrentHP -= amount;
        CurrentHP = Mathf.Max(CurrentHP, 0);
        DamagePopUpGenerator.Instance.CreatePopUp(transform.position, amount.ToString(), Color.red);

        AudioManager.Instance.PlaySoundFXClip(damageSoundClip, transform, 1f);
        // 피격 파티클 소환
        PoolManager.Instance.SpawnFromPool(
            "HitEffect",
            transform.position,
            Quaternion.identity
        );

        if (hpBar != null)
        {
            hpBar.UpdateHPBar((float)CurrentHP / MaxHP);
        }

        if (CurrentHP <= 0)
        {
            DropItem();
            stateMachine.ChangeState(ENEMY_STATE.Die);
        }
        else
        {
            stateMachine.ChangeState(ENEMY_STATE.Hit);
        }
    }

    private void DropItem()
    {
        GameObject drop = AssetManager.Instance.GetRandomDropItem();

        if (drop != null)
            Instantiate(drop, transform.position + Vector3.up * 0.5f, Quaternion.identity);
    }

    public void ChangeState(ENEMY_STATE nextState)
    {
        stateMachine.ChangeState(nextState);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public abstract void InitStateMachine();
}
