using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public float explosionDelay = 2f;
    public float explosionRadius = 3f;
    public float damage = 30f;
    public GameObject explosionEffect;

    private void Start()
    {
        Invoke(nameof(Explode), explosionDelay);
    }

    private void Explode()
    {
        // 이펙트
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 피해 입히기
        Collider[] targets = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var col in targets)
        {
            IDamaged target = col.GetComponent<IDamaged>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
