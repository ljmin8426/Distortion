using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : PoolObject
{
    public float speed = 15f;
    public float damage = 10f;
    public float lifeTime = 5f;

    private Rigidbody rb;
    private float spawnTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        spawnTime = Time.time;
        rb.linearVelocity = Vector3.zero;
    }

    private void Update()
    {
        if (Time.time - spawnTime >= lifeTime)
        {
            PoolManager.Instance.ReturnToPool(this);
        }
    }

    public void Launch(Vector3 direction, float speedOverride)
    {
        speed = speedOverride;

        rb.linearVelocity = direction.normalized * speed;
        spawnTime = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss")) return;

        if (other.CompareTag("Ground") || other.CompareTag("Wall"))
            PoolManager.Instance.ReturnToPool(this);

        var damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, gameObject);
            PoolManager.Instance.ReturnToPool(this);
        }
    }
}
