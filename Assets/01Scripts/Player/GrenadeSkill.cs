using UnityEngine;

public class GrenadeSkill : BaseSkill
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float upwardForce = 5f;

    public override void Activate(GameObject attacker)
    {
        if (TryUseSkill())
        {
            if (grenadePrefab == null)
            {
                Debug.LogWarning("Grenade prefab is not assigned.");
                return;
            }

            // 발사 위치 및 방향
            Transform firePoint = attacker.transform;
            Vector3 spawnPosition = firePoint.position + firePoint.forward + Vector3.up * 1.0f;
            Quaternion spawnRotation = Quaternion.identity;

            // 수류탄 생성
            GameObject grenade = Instantiate(grenadePrefab, spawnPosition, spawnRotation);

            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 launchDir = firePoint.forward * launchForce + Vector3.up * upwardForce;
                rb.AddForce(launchDir, ForceMode.VelocityChange);
            }

            StartCoroutine(CooldownRoutine());
        }
    }
}
