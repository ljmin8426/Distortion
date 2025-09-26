using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossController : PoolObject, IDamageable
{
    public enum BossStage
    { 
        Idle, 
        Stage1, 
        Stage2, 
        Dead 
    }

    [SerializeField] private BossId id;

    [Header("Sound Clips")]
    [SerializeField] private AudioClip phase1Sound;
    [SerializeField] private AudioClip phase1AttackClip;
    [SerializeField] private AudioClip phase2Sound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Phase1 Settings")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float wakeUpDuration = 2f;
    [SerializeField] private float volleyInterval = 5f;

    [Header("Missile Attack")]
    [SerializeField] private Transform missileSpawnPoint;
    [SerializeField] private int missilesPerVolley = 7;
    [SerializeField] private float spreadAngle = 60f;
    [SerializeField] private float missileSpeed = 15f;
    [SerializeField] private string missilePoolTag = "Missile";

    [Header("Phase2 Settings")]
    [SerializeField] private float phase2HPThreshold = 0.66f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpDuration = 1f;

    [Header("Attack Range Settings")]
    [SerializeField] private GameObject attackRangePrefab;
    [SerializeField] private float attackRangeDuration = 1f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRangeRadius = 2f;

    private bool hasAttacked = false;
    private float curHP;
    private bool isDead;
    private bool isPhase2 = false;

    private Transform target;
    private NavMeshAgent agent;
    private Animator animator;
    private BossData bossData;
    private BossStage currentStage;

    public event Action<BossController> OnBossDie;
    public event Action<BossController> OnFightReady;
    public event Action<float, float> OnBossHpChanged;

    private Coroutine stage1Coroutine;
    private Coroutine stage2Coroutine;

    private int isGround = Animator.StringToHash("isGround");
    private int isAttack = Animator.StringToHash("isAttack");
    private int isJump = Animator.StringToHash("isJump");
    private int isWakeUp = Animator.StringToHash("isWakeUp");
    private int isRoar = Animator.StringToHash("isRoar");
    private int isDie = Animator.StringToHash("isDie");
    private int moveX = Animator.StringToHash("moveX");
    private int moveY = Animator.StringToHash("moveY");
    public float JumpHeight => jumpHeight;
    public float JumpDuration => jumpDuration;

    public BossData BossData => bossData;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        InitBoss();
    }

    private void Update()
    {
        if (isDead) return;
        animator.SetBool(isGround, agent.isOnNavMesh && Mathf.Abs(agent.velocity.y) < 0.01f);
    }

    #region Initialization
    public void InitBoss()
    {
        gameObject.layer = LayerMask.NameToLayer("Boss");
        DataManager.Instance.GetBossData((int)id, out bossData);

        curHP = bossData.maxHP;
        target = GameObject.FindWithTag("Player")?.transform;
        isDead = false;
        currentStage = BossStage.Idle;
        OnBossHpChanged?.Invoke(curHP, bossData.maxHP);

        StartCoroutine(IdleRoutine());
    }

    private IEnumerator IdleRoutine()
    {
        while (currentStage == BossStage.Idle && !isDead)
        {
            if (target != null && Vector3.Distance(transform.position, target.position) <= detectionRange)
            {
                animator.SetTrigger(isWakeUp);
                currentStage = BossStage.Stage1;
                yield return YieldCache.WaitForSeconds(wakeUpDuration);
                StartFight();
                yield break;
            }
            yield return null;
        }
    }
    #endregion

    #region Phase1
    public void StartFight()
    {
        if (stage1Coroutine != null) return;

        currentStage = BossStage.Stage1;
        OnFightReady?.Invoke(this);

        if (phase1Sound != null)
            AudioManager.Instance.PlaySoundFXClip(phase1Sound, transform, 1f);

        stage1Coroutine = StartCoroutine(Stage1Attack());
    }

    private IEnumerator Stage1Attack()
    {
        while (currentStage == BossStage.Stage1 && !isDead)
        {
            animator.SetTrigger(isRoar);
            yield return YieldCache.WaitForSeconds(volleyInterval);
        }
    }

    public void FireConeMissiles()
    {
        AudioManager.Instance.PlaySoundFXClip(phase1AttackClip, transform, 1f);
        Transform sp = missileSpawnPoint != null ? missileSpawnPoint : transform;
        Vector3 centerDir = target != null ? target.position - sp.position : sp.forward;
        centerDir.y = 0f;
        centerDir.Normalize();

        float halfAngle = spreadAngle * 0.5f;
        float step = missilesPerVolley > 1 ? spreadAngle / (missilesPerVolley - 1) : 0;

        for (int i = 0; i < missilesPerVolley; i++)
        {
            float angle = -halfAngle + step * i;
            Quaternion rot = Quaternion.Euler(0f, angle, 0f) * Quaternion.LookRotation(centerDir);
            SpawnSingleMissile(sp.position, Quaternion.Euler(0f, rot.eulerAngles.y, 0f));
        }
    }

    private void SpawnSingleMissile(Vector3 pos, Quaternion rot)
    {
        PoolObject poolObj = PoolManager.Instance.SpawnFromPool(missilePoolTag, pos, rot);
        if (poolObj == null) return;
        Projectile proj = poolObj.GetComponent<Projectile>();
        if (proj != null)
        {
            Vector3 dir = rot * Vector3.forward;
            dir.y = 0f;
            dir.Normalize();
            proj.Launch(dir, missileSpeed);
        }
    }
    #endregion

    #region Damage & Death
    public void TakeDamage(float amount, GameObject attacker)
    {
        if (curHP <= 0 || isDead) return;

        curHP -= amount;
        curHP = Mathf.Max(curHP, 0);
        OnBossHpChanged?.Invoke(curHP, bossData.maxHP);

        PoolManager.Instance?.SpawnFromPool("HitEffect", transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySoundFXClip(hitSound, transform, 1f);
        DamagePopUpGenerator.Instance.CreatePopUp(transform.position, amount.ToString(), Color.red);

        if (!isPhase2 && curHP <= bossData.maxHP * phase2HPThreshold)
        {
            isPhase2 = true;
            StartPhase2();
        }

        if(curHP <= 0)
            Die();
    }

    private void Die()
    {
        currentStage = BossStage.Dead;
        isDead = true;
        animator.SetTrigger(isDie);
        AudioManager.Instance.PlaySoundFXClip(deathSound, transform, 1f);
        OnBossDie?.Invoke(this);
    }
    #endregion

    #region Phase2
    private void StartPhase2()
    {
        StopAllCoroutines();
        if (stage2Coroutine != null) return;
        stage2Coroutine = StartCoroutine(Phase2Routine());
    }

    private IEnumerator Phase2Routine()
    {
        currentStage = BossStage.Stage2;

        while (!isDead && currentStage == BossStage.Stage2)
        {
            yield return StartCoroutine(JumpToTargetPredictive(jumpHeight, jumpDuration, true));
            FireConeMissiles();

            yield return StartCoroutine(WaitIdle(2f));

            float trackTime = 20f;
            float elapsed = 0f;
            hasAttacked = false;

            while (elapsed < trackTime && !isDead)
            {
                if (target == null) break;

                float distance = Vector3.Distance(transform.position, target.position);

                if (distance > 4f && !hasAttacked)
                    MoveAndAnimateTowardTarget(target);
                else if (!hasAttacked)
                {
                    AttackPlayer();
                    hasAttacked = true;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            yield return YieldCache.WaitForSeconds(1f);
        }
    }

    private IEnumerator WaitIdle(float time)
    {
        float elapsed = 0f;
        while (elapsed < time && !isDead)
        {
            animator.SetFloat(moveX, 0f);
            animator.SetFloat(moveY, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void AttackPlayer()
    {
        if (target == null) return;
        LookAtTarget(target);

        animator.SetFloat(moveX, 0f);
        animator.SetFloat(moveY, 0f);

        OnHit();
        animator.SetTrigger(isAttack);

        StartCoroutine(ResetAttackFlagAfterDelay(3f));
    }

    public void OnHit()
    {
        if (attackRangePrefab == null) return;

        Vector3 attackPos = transform.position + transform.forward * 5f;
        GameObject range = Instantiate(attackRangePrefab, attackPos, Quaternion.identity);

        AttackRange ar = range.GetComponent<AttackRange>();
        if (ar != null)
        {
            ar.damage = attackDamage;
            ar.radius = attackRangeRadius;
            ar.duration = attackRangeDuration;
        }
    }

    private IEnumerator ResetAttackFlagAfterDelay(float delay)
    {
        yield return YieldCache.WaitForSeconds(delay);
        hasAttacked = false;
    }

    private void MoveAndAnimateTowardTarget(Transform target)
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.01f)
        {
            animator.SetFloat(moveX, 0f);
            animator.SetFloat(moveY, 0f);
            return;
        }

        Vector3 moveDir = dir.normalized;
        transform.position += moveDir * agent.speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * 5f);

        Vector3 localMove = transform.InverseTransformDirection(moveDir);
        animator.SetFloat(moveX, localMove.x);
        animator.SetFloat(moveY, localMove.z);
    }
    #endregion

    #region Utility
    private void LookAtTarget(Transform targetTransform)
    {
        if (targetTransform == null) return;

        Vector3 dir = targetTransform.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(dir);
    }


    public IEnumerator JumpToTargetPredictive(float height, float duration, bool lookBeforeJump = true)
    {
        if (target == null) yield break;

        animator.SetTrigger(isJump);
        Vector3 startPos = transform.position;

        if (lookBeforeJump) LookAtTarget(target);

        Rigidbody playerRb = target.GetComponent<Rigidbody>();
        Vector3 predictedPos = target.position;
        if (playerRb != null) predictedPos += playerRb.linearVelocity * duration;

        agent.enabled = false;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Vector3 horizontalPos = Vector3.Lerp(startPos, predictedPos, t);
            float yOffset = 4f * height * t * (1 - t);
            transform.position = new Vector3(horizontalPos.x, horizontalPos.y + yOffset, horizontalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = predictedPos;
        agent.Warp(transform.position);
        agent.enabled = true;
    }
    #endregion
}



