using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private int damage = 100;

    private float riseHeight = 40f;
    private float riseDuration = 0.5f;

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
            transform.position = Vector3.MoveTowards(transform.position, riseTargetPos, riseHeight / riseDuration * Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, Vector3.up, Time.deltaTime * rotationSpeed);

            if (Vector3.Distance(transform.position, riseTargetPos) < 0.1f)
            {
                isHoming = true;
            }
        }
        else
        {
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
            if (col.CompareTag("Boss"))
            {
                float dist = Vector3.Distance(position, col.transform.position);
                if (dist < bossDist)
                {
                    bossDist = dist;
                    bossTarget = col.transform;
                }
            }
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
            if (other.TryGetComponent(out IDamageable damaged))
            {
                damaged.TakeDamage(damage, gameObject);
                Destroy(gameObject);
            }
        }
    }
}
