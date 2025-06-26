using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        WeaponManager wm = other.GetComponentInChildren<WeaponManager>();
        if (wm == null) return;

        WeaponBase weapon = GetComponent<WeaponBase>();
        if (weapon == null) return;

        wm.PickupAndEquip(weapon);

        // 이 무기 오브젝트는 플레이어 손으로 옮겨졌으니, Pickup 스크립트는 제거
        Destroy(this);
    }
}
