using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Weapon_Type CurrentWeaponType { get; private set; }


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

        EquipWeapon(Weapon_Type.Melee);
    }

    public void EquipWeapon(Weapon_Type type)
    {
        CurrentWeaponType = type;

        switch (type)
        {
            case Weapon_Type.Melee:
                weapon.gameObject.SetActive(true);
                CurWeapon = weapon;
                playerCtrl.Animator.runtimeAnimatorController = CurWeapon.WeaponAnimator;
                break;
        }
    }
}
