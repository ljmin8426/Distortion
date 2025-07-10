using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private Transform target;

    [SerializeField] private int damage = 100;

    [Header("Phase 1: Rising")]
    public float riseHeight = 3f;
    public float riseDuration = 0.5f;

    [Header("Phase 2: Homing")]
    public float homingSpeed = 10f;
    public float rotationSpeed = 10f;
    public float maxLifetime = 5f;

    private Vector3 startPos;
    private Vector3 riseTargetPos;
    private float timer;
    private bool isHoming = false;

    private TrailRenderer trail;

    public void Initialize(Transform target)
    {
        this.target = target;
        startPos = transform.position;
        riseTargetPos = startPos + Vector3.up * riseHeight;
    }

    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }

    private void Start()
    {
        timer = 0f;
        trail?.Clear();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > maxLifetime)
        {
            Destroy(gameObject);
            return;
        }

        // 타겟이 없거나 죽었으면 재탐색
        if (target == null || !IsTargetAlive(target))
        {
            target = FindPriorityTarget(transform.position, 20f);
            if (target == null)
            {
                // 재탐색 실패하면 직진하거나 삭제할 수도 있음
                Destroy(gameObject);
                return;
            }
        }

        if (!isHoming)
        {
            // 상승 중
            transform.position = Vector3.MoveTowards(transform.position, riseTargetPos, riseHeight / riseDuration * Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, Vector3.up, Time.deltaTime * rotationSpeed);

            if (Vector3.Distance(transform.position, riseTargetPos) < 0.1f)
            {
                isHoming = true;
            }
        }
        else
        {
            // 유도 중
            Vector3 dir = (target.position - transform.position).normalized;
            transform.position += dir * homingSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * rotationSpeed);
        }
    }
    private bool IsTargetAlive(Transform targetTransform)
    {
        if (targetTransform == null)
            return false;

        var enemyAI = targetTransform.GetComponent<EnemyAI>();
        if (enemyAI != null)
            return enemyAI.IsAlive();

        //var bossAI = targetTransform.GetComponent<BossAI>(); // 보스 AI도 비슷하게 구현했다고 가정
        //if (bossAI != null)
        //    return bossAI.IsAlive();

        return false; // 컴포넌트가 없으면 안전하게 죽은걸로 간주
    }

    private Transform FindPriorityTarget(Vector3 position, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        Transform bossTarget = null;
        float bossDist = float.MaxValue;

        Transform enemyTarget = null;
        float enemyDist = float.MaxValue;

        foreach (var col in colliders)
        {
            // Boss 우선 찾기
            if (col.CompareTag("Boss"))
            {
                float dist = Vector3.Distance(position, col.transform.position);
                if (dist < bossDist)
                {
                    bossDist = dist;
                    bossTarget = col.transform;
                }
            }
            // 그 다음 Enemy 찾기
            else if (col.CompareTag("Enemy"))
            {
                float dist = Vector3.Distance(position, col.transform.position);
                if (dist < enemyDist)
                {
                    enemyDist = dist;
                    enemyTarget = col.transform;
                }
            }
        }

        if (bossTarget != null)
            return bossTarget;

        return enemyTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            // TODO: 데미지 처리 또는 폭발 이펙트
            Destroy(gameObject);
            if (other.TryGetComponent(out IDamaged damaged))
            {
                damaged.TakeDamage(damage);
            }
        }
    }
}
