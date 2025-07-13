using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    // 데미지
    private int damage = 100;

    // 상승
    private float riseHeight = 40f;
    private float riseDuration = 0.5f;

    // 추적
    private float homingSpeed = 30f;
    private float rotationSpeed = 10f;
    private float maxLifetime = 5f;

    private Transform target;
    private TrailRenderer trail;

    private Vector3 startPos;
    private Vector3 riseTargetPos;
    private float timer;
    private bool isHoming = false;

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
        if ( target == null )
        {
            target = FindPriorityTarget(transform.position, 20f);
            if (target == null)
            {
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
            else if (col.CompareTag("EnemyHead"))
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
            if (other.TryGetComponent(out IDamaged damaged))
            {
                damaged.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
