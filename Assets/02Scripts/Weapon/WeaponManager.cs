using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Handler")]
    [SerializeField] private Transform weaponHolder;

    [Header("Weapon Prefab")]
    [SerializeField] private GameObject meleeWeaponPrefab;

    private BaseWeapon meleeWeapon;

    private PlayerController playerController;

    public BaseWeapon CurWeapon { get; private set; }
    public WEAPON_TYPE CurrentWeaponType { get; private set; }


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        Initialized();
    }

    public void Initialized()
    {
        meleeWeapon = Instantiate(meleeWeaponPrefab, weaponHolder).GetComponent<BaseWeapon>();

        meleeWeapon.gameObject.SetActive(false);

        EquipWeapon(WEAPON_TYPE.Melee);
    }

    public void EquipWeapon(WEAPON_TYPE type)
    {
        CurrentWeaponType = type;

        switch (type)
        {
            case WEAPON_TYPE.Melee:
                meleeWeapon.gameObject.SetActive(true);
                CurWeapon = meleeWeapon;
                playerController.Animator.runtimeAnimatorController = CurWeapon.WeaponAnimator;
                break;
        }
    }
}
