using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        WeaponManager wm = other.GetComponentInChildren<WeaponManager>();
        if (wm == null) return;

        GameObject obj = gameObject;
        if (obj == null) return;

        wm.PickupAndEquip(obj);

        Destroy(this);
    }
}
