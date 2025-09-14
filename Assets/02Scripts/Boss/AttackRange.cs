using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public float radius = 2f;
    public float damage = 10f;
    public float duration = 1f;

    private void Start()
    {
        // duration 후 데미지 적용 및 오브젝트 삭제
        Invoke(nameof(DealDamageAndDestroy), duration);
    }

    private void DealDamageAndDestroy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hits)
        {
            IDamageable dmg = hit.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage, null);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
