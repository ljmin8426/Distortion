using System.Collections;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;

    [SerializeField] protected RuntimeAnimatorController weaponAnimator;

    public RuntimeAnimatorController WeaponAnimator => weaponAnimator;
    public WeaponData WeaponData => weaponData;

    public abstract void Attack();
    public abstract void Skill();
}

