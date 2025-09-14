using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour, IDamageable
{
    public enum BossStage { Idle, Stage1, Stage2, Dead }

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

    [Header("Landing Warning")]
    [SerializeField] private GameObject landingWarningPrefab;
    [SerializeField] private float warningDuration = 1f;

    [SerializeField] private GameObject attackRangePrefab;  // 공격 범위 표시 Prefab
    [SerializeField] private float attackRangeDuration = 1f; // 공격 범위 표시 시간
    private bool hasAttacked = false; // 공격 한 번만 실행 체크

    [SerializeField] private float attackDamage = 10f;   // 공격 데미지
    [SerializeField] private float attackRangeRadius = 2f; // 공격 범위 반지름

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

    public float JumpHeight => jumpHeight;
    public float JumpDuration => jumpDuration;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        InitBoss();
    }

    private void Update()
    {
        if (isDead) return;

        // agent 기준으로 땅 체크
        bool isGrounded = agent.isOnNavMesh && Mathf.Abs(agent.velocity.y) < 0.01f;
        animator.SetBool("isGround", isGrounded);
    }

    #region Boss Initialization
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
                animator.SetTrigger("isWakeUp");
                currentStage = BossStage.Stage1;
                yield return new WaitForSeconds(wakeUpDuration);
                StartFight();
                yield break;
            }
            yield return null;
        }
    }
    #endregion

    #region Phase1 Attack
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
            animator.SetTrigger("isRoar");
            yield return new WaitForSeconds(volleyInterval);
        }
    }

    public void FireConeMissiles()
    {
        AudioManager.Instance.PlaySoundFXClip(phase1AttackClip, transform, 1f);
        Transform sp = missileSpawnPoint != null ? missileSpawnPoint : transform;

        Vector3 centerDir = target != null ? target.position - sp.position : sp.forward;
        centerDir.y = 0f;
        centerDir.Normalize();

        if (missilesPerVolley <= 1)
        {
            SpawnSingleMissile(sp.position, Quaternion.LookRotation(centerDir));
            return;
        }

        float halfAngle = spreadAngle * 0.5f;
        float step = spreadAngle / (missilesPerVolley - 1);

        for (int i = 0; i < missilesPerVolley; i++)
        {
            float angle = -halfAngle + step * i;
            Quaternion rot = Quaternion.Euler(0f, angle, 0f) * Quaternion.LookRotation(centerDir);
            rot = Quaternion.Euler(0f, rot.eulerAngles.y, 0f);
            SpawnSingleMissile(sp.position, rot);
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

        if (!isPhase2 && curHP <= bossData.maxHP * phase2HPThreshold)
        {
            isPhase2 = true;
            StartPhase2();
        }

        if (curHP <= 0)
            StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        currentStage = BossStage.Dead;
        isDead = true;
        animator.SetTrigger("Die");
        AudioManager.Instance.PlaySoundFXClip(deathSound, transform, 1f);
        yield return new WaitForSeconds(3f);
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
            // 1. 점프 수행
            yield return StartCoroutine(JumpToTargetPredictive(jumpHeight, jumpDuration, lookBeforeJump: true));

            FireConeMissiles();

            // === 착지 후 대기 2초 ===
            float waitTime = 2f;
            float waited = 0f;
            while (waited < waitTime && !isDead)
            {
                animator.SetFloat("moveX", 0f);
                animator.SetFloat("moveY", 0f);

                waited += Time.deltaTime;
                yield return null;
            }

            // 2. 플레이어 추적 및 공격
            float trackTime = 20f;
            float elapsed = 0f;
            hasAttacked = false; // 점프 후 공격 초기화

            while (elapsed < trackTime && !isDead)
            {
                if (target != null)
                {
                    float distance = Vector3.Distance(transform.position, target.position);

                    if (distance > 4f)
                    {
                        // 플레이어 추적
                        if (!hasAttacked) // 공격 중이 아니면 추적
                            MoveAndAnimateTowardTarget(target);
                    }
                    else if (!hasAttacked)
                    {
                        // 플레이어 근접: 공격 시작
                        AttackPlayer();
                        hasAttacked = true;
                    }
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            // 3. 잠시 대기 후 다음 점프 반복
            yield return new WaitForSeconds(1f);
        }
    }

    private void AttackPlayer()
    {
        if (target == null) return;

        // 1. 플레이어 방향 회전
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(dir);

        // 2. 추적 멈춤
        animator.SetFloat("moveX", 0f);
        animator.SetFloat("moveY", 0f);

        // 3. 공격 애니메이션 재생
        animator.SetTrigger("isAttack");

        // 4. 공격 범위 Prefab 생성 (애니메이션 이벤트에서 바로 활성화 가능)
        if (attackRangePrefab != null)
        {
            Vector3 attackPos = transform.position + transform.forward * 5f;
            GameObject range = Instantiate(attackRangePrefab, attackPos, Quaternion.identity);

            AttackRange ar = range.GetComponent<AttackRange>();
            if (ar != null)
            {
                ar.damage = attackDamage;
                ar.radius = attackRangeRadius;
                ar.duration = attackRangeDuration; // Prefab에서 설정 가능
            }
        }

        // 5. 공격 후 일정 시간 추적 재개
        StartCoroutine(ResetAttackFlagAfterDelay(1f));
    }

    // OnHit 애니메이션 이벤트
    public void OnHit()
    {
        if (attackRangePrefab == null || target == null) return;

        Vector3 attackPos = transform.position + transform.forward * 5f;
        GameObject range = Instantiate(attackRangePrefab, attackPos, Quaternion.identity);

        // AttackRange 값 전달
        AttackRange ar = range.GetComponent<AttackRange>();
        if (ar != null)
        {
            ar.damage = attackDamage;
            ar.radius = attackRangeRadius;
            ar.duration = attackRangeDuration;
        }
    }


    public void SpawnAttackRange()
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
        yield return new WaitForSeconds(delay);
        hasAttacked = false;
    }


    private void MoveAndAnimateTowardTarget(Transform target)
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f)
        {
            animator.SetFloat("moveX", 0f);
            animator.SetFloat("moveY", 0f);
            return;
        }

        Vector3 moveDir = dir.normalized;

        // 실제 이동
        transform.position += moveDir * agent.speed * Time.deltaTime;

        // 플레이어 방향 회전
        Quaternion targetRot = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);

        // Animator 파라미터 세팅 (보스 로컬 방향 기준)
        Vector3 localMove = transform.InverseTransformDirection(moveDir);
        animator.SetFloat("moveX", localMove.x);
        animator.SetFloat("moveY", localMove.z);
    }

    #endregion

    #region Utility Functions
    // 플레이어 바라보기 (Y축 고정)
    private void LookAtTarget(Transform targetTransform)
    {
        if (targetTransform == null) return;
        Vector3 dir = targetTransform.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }
    }

    // 점프 + 착지 경고 + 플레이어 이동 예측
    public IEnumerator JumpToTargetPredictive(float height, float duration, bool lookBeforeJump = true)
    {
        if (target == null) yield break;

        animator.SetTrigger("isJump");

        Vector3 startPos = transform.position;

        // 플레이어 바라보기
        if (lookBeforeJump) LookAtTarget(target);

        // 플레이어 이동 예측: 단순히 이동 방향과 속도 기반
        Rigidbody playerRb = target.GetComponent<Rigidbody>();
        Vector3 predictedPos = target.position;
        if (playerRb != null)
        {
            predictedPos += playerRb.linearVelocity * duration; // duration 동안 이동 예상
        }

        // 착지 경고 표시
        ShowLandingWarning(predictedPos);

        // Agent 비활성화
        agent.enabled = false;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // XZ 선형 이동
            Vector3 horizontalPos = Vector3.Lerp(startPos, predictedPos, t);

            // Y 포물선
            float yOffset = 4f * height * t * (1 - t);
            transform.position = new Vector3(horizontalPos.x, horizontalPos.y + yOffset, horizontalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 착지
        transform.position = predictedPos;

        // Agent 위치 보정 후 재활성화
        agent.Warp(transform.position);
        agent.enabled = true;
    }

    private void ShowLandingWarning(Vector3 landingPos)
    {
        if (landingWarningPrefab == null) return;
        GameObject warning = Instantiate(landingWarningPrefab, landingPos, Quaternion.identity);
        Destroy(warning, warningDuration);
    }
    #endregion
}
