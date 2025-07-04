using UnityEngine;

public class TwoHandSword : BaseWeapon, IMeleeWeapon
{
    private BoxCollider meleeArea;
    private TrailRenderer trailRenderer;


    private void Awake()
    {
        meleeArea = GetComponent<BoxCollider>();
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

    public override void Skill() { }
    public override void Attack() { }
}
