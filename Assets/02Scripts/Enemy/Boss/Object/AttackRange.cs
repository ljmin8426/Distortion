using UnityEngine;

public class AttackRange : PoolObject
{
    public float radius = 2f;
    public float damage = 10f;
    public float duration = 1f;

    private void Start()
    {
        Invoke(nameof(DealDamageAndDestroy), duration);
    }

    private void DealDamageAndDestroy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Boss"))
                continue;

            IDamageable dmg = hit.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage, null);
            }
        }

        ReturnToPool();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
