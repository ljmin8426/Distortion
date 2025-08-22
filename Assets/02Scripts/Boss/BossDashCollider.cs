using UnityEngine;

public class BossDashCollider : MonoBehaviour
{
    private BossController boss;

    private void Awake()
    {
        boss = GetComponentInParent<BossController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (boss == null || !boss.IsDashing()) return;

        if (other.CompareTag("Player") && other.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(25, gameObject);
            boss.StopDash(); // 보스에게 종료 명령
        }
    }
}
