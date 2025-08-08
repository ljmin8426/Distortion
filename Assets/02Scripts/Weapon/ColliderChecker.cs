using System.Collections.Generic;
using UnityEngine;

public class ColliderChecker : MonoBehaviour
{
    private HashSet<Collider> damagedTargets = new HashSet<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            return;

        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage((int)PlayerStatManager.Instance.ATK + 45, gameObject);
            damagedTargets.Add(other);
        }
    }

    public void ClearDamagedTargets()
    {
        damagedTargets.Clear();
    }
}
