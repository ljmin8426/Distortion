using System.Collections;
using UnityEngine;

public class Gun : BaseWeapon
{
    [Header("Gun Settings")]
    [SerializeField] private Transform firePoint;      // 총알 발사 위치
    [SerializeField] private float fireRange = 100f;   // 사거리
    [SerializeField] private LayerMask hitLayers;      // 타격 대상 레이어

    private bool isFiring = false;

    private void Awake()
    {
        if (firePoint == null)
        {
            Debug.LogError("FirePoint is not assigned.");
        }
    }

    public override void Attack()
    {
        if (!isFiring)
        {
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        isFiring = true;

        // 피격 판정
        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, fireRange, hitLayers))
        {
            IDamaged damaged = hit.collider.GetComponent<IDamaged>();
            if (damaged != null)
            {
                damaged.TakeDamage(weaponData.attackDamage);
            }

            // TODO: 피격 이펙트 생성 가능
            // Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }

        // TODO: 총알 발사 이펙트 / 사운드

        yield return new WaitForSeconds(1f / 0); // 예: 2이면 초당 2발

        isFiring = false;
    }

    public override void Skill()
    {
        throw new System.NotImplementedException();
    }
}
