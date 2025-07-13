using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum BossPhase { Phase1, Phase2 }
public enum BossState { Idle, Phase1, Phase2, Die }

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(AudioSource))]
public class BossController : MonoBehaviour, IDamaged
{
    [Header("보스 스탯")]
    public int maxHP = 500;
    public int currentHP;
    public float moveSpeed = 4f;

    [Header("페이즈 전환")]
    public BossPhase currentPhase = BossPhase.Phase1;
    public AudioClip phase2Sound;

    [Header("참조")]
    public Transform player;
    public Transform firePointLeft;
    public Transform firePointRight;
    public Transform firePoint;
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;

    [Header("프리팹")]
    public GameObject laserWarningPrefab;
    public GameObject laserPrefab;
    public GameObject areaWarningPrefab;
    public GameObject areaDamageZonePrefab; // 동그란 데미지 존 프리팹
    public GameObject bulletPrefab;
    [SerializeField] private GameObject dashWarningPrefab; // 돌진 경고 프리팹
    [SerializeField] private GameObject dashColliderObj;
    [SerializeField] private float areaZoneRadius = 10f; // 범위 증가
    [SerializeField] private GameObject dashWarningObj;

    private Vector3 dashTargetPos;
    private BossState currentState;
    private float stateTimer;
    private bool isLeftNext = true;
    private bool isDashing = false;
    private Coroutine bulletRoutine = null;

    public bool IsDashing() => isDashing;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (dashWarningObj != null)
            dashWarningObj.SetActive(false);
        currentHP = maxHP;
        agent.speed = moveSpeed;
        ChangeState(BossState.Idle);
    }

    void Update()
    {
        if (currentState != BossState.Die && player != null)
        {
            agent.SetDestination(player.position);
            Vector3 moveDir = agent.desiredVelocity;
            float speed = moveDir.magnitude;

            if (bulletRoutine == null)
                animator.SetFloat("Movespeed", speed);

            if (moveDir != Vector3.zero && bulletRoutine == null)
            {
                Quaternion lookRot = Quaternion.LookRotation(moveDir.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
            }

            if (bulletRoutine != null)
            {
                Vector3 lookDir = player.position - transform.position;
                lookDir.y = 0;
                if (lookDir != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(lookDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
                }
            }
        }

        switch (currentState)
        {
            case BossState.Idle: HandleIdle(); break;
            case BossState.Phase1: HandlePhase1(); break;
            case BossState.Phase2: HandlePhase2(); break;
        }
    }

    private void ChangeState(BossState nextState)
    {
        currentState = nextState;
        stateTimer = 0f;

        if (nextState == BossState.Idle)
        {
            agent.isStopped = true;
            animator.SetFloat("Movespeed", 0f);
        }
        else if (nextState == BossState.Die)
        {
            StartCoroutine(HandleDeath());
        }
        else
        {
            agent.isStopped = false;
        }
    }

    private void HandleIdle()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < 15f)
        {
            ChangeState(BossState.Phase1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentState == BossState.Die) return;

        currentHP = Mathf.Max(currentHP - damage, 0);
        if (currentHP <= 0)
        {
            ChangeState(BossState.Die);
        }
        else if (currentPhase == BossPhase.Phase1 && currentHP <= maxHP / 2)
        {
            EnterPhase2();
        }
    }

    private void EnterPhase2()
    {
        currentPhase = BossPhase.Phase2;
        if (phase2Sound) audioSource.PlayOneShot(phase2Sound);
        if (animator) animator.SetTrigger("Is2Phase");
        StartCoroutine(EnterPhase2Sequence());
    }

    private IEnumerator EnterPhase2Sequence()
    {
        agent.isStopped = true;
        animator.SetFloat("Movespeed", 0f);
        yield return new WaitForSeconds(3f);
        agent.isStopped = false;
        ChangeState(BossState.Phase2);
    }

    private IEnumerator HandleDeath()
    {
        agent.isStopped = true;
        animator.SetFloat("Movespeed", 0f);
        animator.SetTrigger("IsDie");
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void HandlePhase1()
    {
        if (bulletRoutine != null) return;

        stateTimer += Time.deltaTime;
        if (stateTimer >= 3f)
        {
            int pattern = Random.Range(0, 2); // 0: 총알, 1: 레이저
            if (pattern == 0)
                bulletRoutine = StartCoroutine(FireRapidBullets());
            else
                StartCoroutine(FireSingleLaser());
            stateTimer = 0f;
        }
    }

    private void HandlePhase2()
    {
        if (bulletRoutine != null || isDashing) return;

        stateTimer += Time.deltaTime;

        if (stateTimer >= 4f)
        {
            int pattern = Random.Range(0, 4);
            switch (pattern)
            {
                case 0: StartCoroutine(DashSequence()); break;
                case 1: StartCoroutine(FireAreaDamageZones()); break; // 변경된 패턴
                case 2: StartCoroutine(FireSingleLaser()); break;
                case 3: bulletRoutine = StartCoroutine(FireRapidBullets()); break;
            }
            stateTimer = 0f;
        }
    }

    private IEnumerator FireRapidBullets(int bulletCount = 15, float interval = 0.2f)
    {
        agent.isStopped = true;
        animator.SetFloat("Movespeed", 0f);
        animator.SetTrigger("IsShoot");

        for (int i = 0; i < bulletCount; i++)
        {
            Transform firePoint = isLeftNext ? firePointLeft : firePointRight;
            isLeftNext = !isLeftNext;

            if (firePoint == null || player == null) break;

            Vector3 dir = (player.position + Vector3.up - firePoint.position).normalized;
            Quaternion rot = Quaternion.LookRotation(dir);

            Instantiate(bulletPrefab, firePoint.position, rot);
            yield return new WaitForSeconds(interval);
        }

        agent.isStopped = false;
        bulletRoutine = null;
        animator.SetFloat("Movespeed", 0.5f);
    }

    private IEnumerator FireSingleLaser()
    {
        if (player == null) yield break;

        Vector3 dir = (player.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);

        Vector3 spawnPos = player.position;

        Instantiate(laserWarningPrefab, spawnPos, rot);
        yield return new WaitForSeconds(1.5f);
        Instantiate(laserPrefab, spawnPos, rot);
    }

    private IEnumerator FireAreaDamageZones(int count = 8, float delay = 1.5f)
    {
        for (int i = 0; i < count; i++)
        {
            // 반지름 범위 내에서 무작위 위치 선택 (XZ 평면 기준)
            Vector2 offset2D = Random.insideUnitCircle * areaZoneRadius;
            Vector3 randomPos = player.position + new Vector3(offset2D.x, 0, offset2D.y);

            Instantiate(areaWarningPrefab, randomPos, Quaternion.identity);
            StartCoroutine(SpawnDelayedDamageZone(randomPos, delay));
        }

        yield return null;
    }

    private IEnumerator SpawnDelayedDamageZone(Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(areaDamageZonePrefab, pos, Quaternion.identity);
    }

    private IEnumerator DashSequence()
    {
        agent.isStopped = true;
        animator.SetFloat("Movespeed", 0f);

        if (player == null) yield break;

        // 1. 돌진 대상 방향과 위치 고정
        Vector3 dashDir = (player.position - transform.position).normalized;
        dashTargetPos = transform.position + dashDir * 20f; // 예: 20m 앞으로 돌진
        Quaternion dashLookRot = Quaternion.LookRotation(dashDir);

        // 2. 경고 표시 + 플레이어 바라보기 유지
        if (dashWarningObj != null)
        {
            dashWarningObj.SetActive(true);
            dashWarningObj.transform.rotation = dashLookRot;
        }

        float timer = 0f;
        while (timer < 2f)
        {
            // 회전만 유지 (플레이어 바라보기)
            if (player != null)
            {
                Vector3 lookDir = player.position - transform.position;
                lookDir.y = 0;
                if (lookDir != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(lookDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // 3. 경고 숨기기
        if (dashWarningObj != null)
            dashWarningObj.SetActive(false);

        // 4. 돌진 시작
        isDashing = true;
        agent.isStopped = true; // NavMeshAgent 비활성화
        animator.SetFloat("Movespeed", 1f);

        if (dashColliderObj != null)
            dashColliderObj.SetActive(true);

        // 5. transform.forward 고정 → Rigidbody 또는 직접 Translate 방식 돌진
        StartCoroutine(DashMovement(dashDir));
    }

    private IEnumerator DashMovement(Vector3 dashDir)
    {
        float dashDuration = 1.5f; // 돌진 시간
        float dashSpeed = 20f;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            transform.position += dashDir * dashSpeed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        StopDash();
    }


    public void StopDash()
    {
        isDashing = false;
        agent.isStopped = true;
        agent.speed = moveSpeed;
        agent.ResetPath();
        animator.SetFloat("Movespeed", 0.5f);

        if (dashColliderObj != null)
            dashColliderObj.SetActive(false);

        stateTimer = 4f;
    }
}
