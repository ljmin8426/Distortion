using UnityEngine;

public class Gun : BaseWeapon
{
    [Header("Gun Settings")]
    [SerializeField] private Transform firePoint;             // 총알 발사 위치

    private float nextFireTime = 0f;

    private void Awake()
    {
        if (firePoint == null)
            Debug.LogError("FirePoint is not assigned.");
    }

    public override void Attack()
    {
        if (Time.time < nextFireTime) return;

        Fire();
        nextFireTime = Time.time + (1f / weaponData.attackSpeed);
    }

    private void Fire()
    {

    }

    public override void Skill()
    {
    }
}
