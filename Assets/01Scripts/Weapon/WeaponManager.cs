using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform weaponHolder; // 오른손 위치 연결

    private IWeapon equippedWeapon;
    private GameObject equippedWeaponObj;

    private IWeapon backupWeapon;
    private GameObject backupWeaponObj;

    public void PickupAndEquip(IWeapon newWeapon)
    {
        GameObject newWeaponObj = ((MonoBehaviour)newWeapon).gameObject;

        // 백업 무기 저장
        if (equippedWeapon != null)
        {
            if (backupWeaponObj != null)
                Destroy(backupWeaponObj); // 기존 백업 무기는 제거

            backupWeapon = equippedWeapon;
            backupWeaponObj = equippedWeaponObj;
        }

        Equip(newWeapon, newWeaponObj);
    }

    public void SwapWeapon()
    {
        if (backupWeapon == null || backupWeaponObj == null) return;

        // 현재 무기 ↔ 백업 무기 스왑
        IWeapon tempWeapon = equippedWeapon;
        GameObject tempObj = equippedWeaponObj;

        Equip(backupWeapon, backupWeaponObj);

        backupWeapon = tempWeapon;
        backupWeaponObj = tempObj;
    }

    private void Equip(IWeapon weapon, GameObject obj)
    {
        equippedWeapon?.OnUnequip();

        obj.transform.SetParent(weaponHolder);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        equippedWeapon = weapon;
        equippedWeaponObj = obj;
        equippedWeapon.OnEquip();
    }


    public void TryAttack()
    {
        if (equippedWeapon != null && equippedWeapon.CanAttack)
        {
            equippedWeapon.Attack();
        }
    }
}
