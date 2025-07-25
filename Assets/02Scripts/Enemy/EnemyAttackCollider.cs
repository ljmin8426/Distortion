using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    private Collider attackCollider;
    private EnemyBase enemyBase;

    private void Awake()
    {
        enemyBase = GetComponentInParent<EnemyBase>();
        attackCollider = GetComponent<Collider>();
        attackCollider.enabled = false;
    }

    public void EnableCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableCollider()
    {
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!attackCollider.enabled) return;

        if (!other.CompareTag("Player")) return;

        if (other.TryGetComponent<IDamaged>(out var damaged))
        {
            damaged.TakeDamage(enemyBase.EnemyData.attackPower);
        }
    }
}
