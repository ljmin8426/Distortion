using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Prefab")]
    [SerializeField] private GameObject meleeWeaponPrefab;
    [SerializeField] private GameObject rangedWeaponPrefab;

    private BaseWeapon meleeWeapon;
    private BaseWeapon rangedWeapon;

    private PlayerController playerController;
    private Transform weaponHolder;

    public BaseWeapon CurWeapon { get; private set; }
    public WEAPON_TYPE CurrentWeaponType { get; private set; }


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void Initialized(Transform hand)
    {
        weaponHolder = hand;

        meleeWeapon = Instantiate(meleeWeaponPrefab, this.weaponHolder).GetComponent<BaseWeapon>();
        rangedWeapon = Instantiate(rangedWeaponPrefab, this.weaponHolder).GetComponent<BaseWeapon>();

        meleeWeapon.gameObject.SetActive(false);
        rangedWeapon.gameObject.SetActive(false);

        EquipWeapon(WEAPON_TYPE.Melee);
    }

    public void EquipWeapon(WEAPON_TYPE type)
    {
        CurrentWeaponType = type;

        switch (type)
        {
            case WEAPON_TYPE.Melee:
                meleeWeapon.gameObject.SetActive(true);
                rangedWeapon.gameObject.SetActive(false);
                CurWeapon = meleeWeapon;
                playerController.Animator.runtimeAnimatorController = CurWeapon.WeaponAnimator;
                break;

            case WEAPON_TYPE.Range:
                meleeWeapon.gameObject.SetActive(false);
                rangedWeapon.gameObject.SetActive(true);
                CurWeapon = rangedWeapon;
                playerController.Animator.runtimeAnimatorController = CurWeapon.WeaponAnimator;
                break;
        }
    }
    public void SwapWeapon()
    {
        EquipWeapon(CurrentWeaponType == WEAPON_TYPE.Melee ? WEAPON_TYPE.Range : WEAPON_TYPE.Melee);
    }

    public void TryAttack()
    {
        CurWeapon?.Attack();
    }
}
