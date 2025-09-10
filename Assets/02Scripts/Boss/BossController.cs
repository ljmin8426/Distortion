using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour, IDamageable
{
    public enum BossStage
    {
        Idle,
        Stage1,
        Stage2,
        Stage3,
        Dead
    }

    [SerializeField] private BossId id;

    [Header("SoundClip")]
    [SerializeField] private AudioClip phase1Sound;
    [SerializeField] private AudioClip phase2Sound;
    [SerializeField] private AudioClip phase3Sound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Phase1")]
    [SerializeField] private float detectionRange;

    private float curHP;
    private bool isDead;


    private Transform target;
    private NavMeshAgent agent;
    private Animator animator;

    private BossData bossData;
    private BossStage currentStage;

    public event Action<BossController> OnBossDie;
    public event Action<BossController> OnFightReady;
    public event Action<float, float> OnBossHpChanged;

    public BossData BossData => bossData;
    public NavMeshAgent Agent => agent;
    public Transform Target => target;
    public Animator Animator => animator;
    public float DetectionRange => detectionRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        InitBoss();
    }

    public void InitBoss()
    {
        gameObject.layer = LayerMask.NameToLayer("Boss");
        DataManager.Instance.GetBossData((int)id, out bossData);

        curHP = bossData.maxHP;
        target = GameObject.FindWithTag("Player")?.transform;

        isDead = false;
        OnBossHpChanged?.Invoke(curHP, bossData.maxHP);

        currentStage = BossStage.Idle;
    }

    public void StartFight()
    {
        currentStage = BossStage.Stage1;
        animator.SetTrigger("StartFight");
        OnFightReady?.Invoke(this);

        StartCoroutine(Stage1Attack());
    }

    public void TakeDamage(float amount, GameObject attacker)
    {
        if (curHP <= 0 || isDead) return;

        curHP -= amount;
        curHP = Mathf.Max(curHP, 0);
        OnBossHpChanged?.Invoke(curHP, bossData.maxHP);

        PoolManager.Instance.SpawnFromPool("HitEffect", transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySoundFXClip(hitSound, transform, 1f);

        if (curHP > 0)
        {
            CheckStageTransition();
        }
        else
        {
            StartCoroutine(Die());
        }
    }

    private void CheckStageTransition()
    {
        if (curHP <= bossData.maxHP * 0.3f && currentStage != BossStage.Stage3)
        {
            StartCoroutine(GoToStage3());
        }
        else if (curHP <= bossData.maxHP * 0.6f && currentStage == BossStage.Stage1)
        {
            GoToStage2();
        }
    }

    private void GoToStage2()
    {
        currentStage = BossStage.Stage2;
        AudioManager.Instance.PlaySoundFXClip(phase2Sound, transform, 1f);
        StopAllCoroutines();
        StartCoroutine(Stage2Attack());
    }

    private IEnumerator GoToStage3()
    {
        currentStage = BossStage.Stage3;
        Debug.Log("Stage 3 Activated!");
        StopAllCoroutines();
        yield return Stage3Attack();
    }
    private IEnumerator Die()
    {
        Debug.Log("Boss Defeated!");
        currentStage = BossStage.Dead;
        isDead = true;
        animator.SetTrigger("Die");

        AudioManager.Instance.PlaySoundFXClip(deathSound, transform, 1f);
        yield return new WaitForSeconds(3f);

        OnBossDie?.Invoke(this);
    }

    #region AttackPatterns
    private IEnumerator Stage1Attack()
    {
        while (currentStage == BossStage.Stage1)
        {
            Debug.Log("Stage 1 Attack Pattern");
            yield return new WaitForSeconds(5f);
        }
    }

    private IEnumerator Stage2Attack()
    {
        while (currentStage == BossStage.Stage2)
        {
            Debug.Log("Stage 2 Attack Pattern");
            yield return new WaitForSeconds(7f);
        }
    }

    private IEnumerator Stage3Attack()
    {
        while (currentStage == BossStage.Stage3)
        {
            Debug.Log("Stage 3 Attack Pattern");
            yield return new WaitForSeconds(10f);
        }
    }
    #endregion
}
