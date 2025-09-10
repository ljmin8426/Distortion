using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Handler")]
    [SerializeField] private Transform weaponHolder;

    [Header("Weapon Prefab")]
    [SerializeField] private GameObject weaponObj;

    private BaseWeapon weapon;

    private PlayerCtrl playerCtrl;

    public BaseWeapon CurWeapon { get; private set; }
    public WEAPON_TYPE CurrentWeaponType { get; private set; }


    private void Awake()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
    }

    private void Start()
    {
        Initialized();
    }

    public void Initialized()
    {
        weapon = Instantiate(weaponObj, weaponHolder).GetComponent<BaseWeapon>();

        weapon.gameObject.SetActive(false);

        EquipWeapon(WEAPON_TYPE.Melee);
    }

    public void EquipWeapon(WEAPON_TYPE type)
    {
        CurrentWeaponType = type;

        switch (type)
        {
            case WEAPON_TYPE.Melee:
                weapon.gameObject.SetActive(true);
                CurWeapon = weapon;
                playerCtrl.Animator.runtimeAnimatorController = CurWeapon.WeaponAnimator;
                break;
        }
    }
}
