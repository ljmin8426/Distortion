using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private List<GameObject> weapons = new List<GameObject>();
    public BaseWeapon CurWeapon {  get; private set; }

    private GameObject curWeaponObject;
    private Transform weaponHolder; 

    public Action<GameObject> OnDeleteWeapon { get; set; }

    public void Initialized(Transform weaponHolder)
    {
        this.weaponHolder = weaponHolder;
    }

    public void PickupAndEquip(GameObject newWeapon)
    {
        if (!weapons.Contains(newWeapon))
        {
            BaseWeapon weaponInfo = newWeapon.GetComponent<BaseWeapon>();

            newWeapon.transform.SetParent(weaponHolder);
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localRotation = Quaternion.identity;
            weapons.Add(newWeapon);
            newWeapon.SetActive(false);

            Equip(weaponInfo, newWeapon);
        }

    }

    public void DeleteWeapon(GameObject weapon)
    {
        if(weapons.Contains(weapon))
        {
            weapons.Remove(weapon);
            OnDeleteWeapon.Invoke(weapon);
        }
    }

    private void Equip(BaseWeapon weapon, GameObject obj)
    {
        obj.transform.SetParent(weaponHolder);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        CurWeapon = weapon;
        curWeaponObject = obj;

        curWeaponObject.SetActive(true);
    }

    public void SwapWeapon(GameObject weapon)
    {
        if(CurWeapon == null)
        {
            curWeaponObject = weapon;
            CurWeapon = weapon.GetComponent<BaseWeapon>();
            curWeaponObject.SetActive(true) ;

            PlayerController pc = GetComponentInParent<PlayerController>();
            pc.Animator.runtimeAnimatorController = CurWeapon.WeaponAnimator;

            return;
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].Equals(weapon))
            {
                curWeaponObject = weapon;
                curWeaponObject.SetActive(true);
                CurWeapon = weapon.GetComponent<BaseWeapon> ();

                PlayerController pc = GetComponentInParent<PlayerController>();
                pc.Animator.runtimeAnimatorController = CurWeapon.WeaponAnimator;

                continue;
            }
            weapons[i].SetActive(false);
        }
    }


    public void TryAttack()
    {
        if (CurWeapon != null)
        {
            CurWeapon.Attack();
        }
    }
}
