using System.Collections;
using UnityEngine;

public class TwoHandSword : WeaponBase
{
    [SerializeField] private BoxCollider meleeArea;
    [SerializeField] private TrailRenderer trailRenderer;

    protected override void Awake()
    {
        base.Awake();

        meleeArea.enabled = false;
        trailRenderer.enabled = false;
    }

    public override void Attack()
    {
        if (TryAttack())
        {
            StartCoroutine(DoAttack());
        }
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


    private IEnumerator DoAttack()
    {
        AnimationEvents.OnAttack?.Invoke();
        meleeArea.enabled = true;
        trailRenderer.enabled = true;

        yield return new WaitForSeconds(0.5f);

        meleeArea.enabled = false;
        trailRenderer.enabled = false;

        yield return StartCoroutine(AttackDelay());
    }
}
