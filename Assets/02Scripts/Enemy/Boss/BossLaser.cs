using UnityEngine;

public class BossLaser : MonoBehaviour
{
    public float duration = 2f;     // 존재 시간
    public int damage = 15;         // 플레이어에게 주는 피해

    private void Start()
    {
        // duration 초 뒤에 자동으로 삭제
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out IDamaged target))
        {
            target.TakeDamage(damage);
        }
    }
}
