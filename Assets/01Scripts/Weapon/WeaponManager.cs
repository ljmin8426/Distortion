using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Prefab")]
    [SerializeField] private GameObject meleeWeaponPrefab;
    [SerializeField] private GameObject rangedWeaponPrefab;

    [Header("Weapon Animator")]
    [SerializeField] private RuntimeAnimatorController meleeAnimator;
    [SerializeField] private RuntimeAnimatorController rangedAnimator;

    private BaseWeapon meleeWeapon;
    private BaseWeapon rangedWeapon;

    private Transform weaponHolder;
    private Animator playerAnimator;

    public BaseWeapon CurWeapon { get; private set; }
    public WEAPON_TYPE CurrentWeaponType { get; private set; }

    public void Initialized(Transform weaponHolder, Animator animator)
    {
        this.weaponHolder = weaponHolder;
        this.playerAnimator = animator;

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
                playerAnimator.runtimeAnimatorController = meleeAnimator;
                break;

            case WEAPON_TYPE.Range:
                meleeWeapon.gameObject.SetActive(false);
                rangedWeapon.gameObject.SetActive(true);
                CurWeapon = rangedWeapon;
                playerAnimator.runtimeAnimatorController = rangedAnimator;
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
