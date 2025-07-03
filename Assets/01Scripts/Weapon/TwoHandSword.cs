using UnityEngine;

public class TwoHandSword : BaseWeapon, IMeleeWeapon
{
    private BoxCollider meleeArea;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        meleeArea = GetComponentInChildren<BoxCollider>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();

        meleeArea.enabled = false;
        trailRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!meleeArea.enabled) return;

        if (other.TryGetComponent(out IDamaged damaged))
        {
            damaged.TakeDamage(weaponData.attackDamage);
        }
    }

    public override void Attack()
    {
        // 기존 공격 로직 (필요 시)
    }

    public void EnableMelee()
    {
        meleeArea.enabled = true;
        trailRenderer.enabled = true;
    }

    public void DisableMelee()
    {
        meleeArea.enabled = false;
        trailRenderer.enabled = false;
    }
}
