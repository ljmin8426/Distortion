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

        // 적 또는 보스 태그만 허용
        if (!other.CompareTag("Enemy") && !other.CompareTag("Boss"))
            return;

        if (other.TryGetComponent(out IDamaged damaged))
        {
            damaged.TakeDamage(weaponData.attackDamage + (int)PlayerStatManager.instance.AttackPower);
        }
    }

    public void EnableMelee()
    {
        meleeArea.enabled = true;
        trailRenderer.enabled = true;
        AudioManager.instance.PlaySFX("Attack");
    }

    public void DisableMelee()
    {
        meleeArea.enabled = false;
        trailRenderer.enabled = false;
    }

    public override void Skill() { }
    public override void Attack() { }
}
