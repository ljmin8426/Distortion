using System.Collections;
using UnityEngine;

public class TwoHandSword : BaseWeapon
{
    [SerializeField] private BoxCollider meleeArea;
    [SerializeField] private TrailRenderer trailRenderer;

    private void Awake()
    {
        meleeArea.enabled = false;
        trailRenderer.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!meleeArea.enabled) return;

        // 적이 IDamageable 인터페이스를 구현한 경우
        IDamaged damaged = other.GetComponent<IDamaged>();
        if (damaged != null)
        {
            damaged.TakeDamage(weaponData.attackDamage);
        }
    }

    public override void Attack()
    {
        StartCoroutine(DoAttack());
    }

    private IEnumerator DoAttack()
    {
        AnimationEvents.OnAttack?.Invoke();
        meleeArea.enabled = true;
        trailRenderer.enabled = true;

        yield return new WaitForSeconds(0.5f);

        meleeArea.enabled = false;
        trailRenderer.enabled = false;
    }
}
