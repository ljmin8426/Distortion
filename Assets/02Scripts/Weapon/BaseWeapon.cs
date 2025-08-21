using System.Collections;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected WeaponDataSO weaponData;

    [SerializeField] protected RuntimeAnimatorController weaponAnimator;

    [SerializeField] protected string dashAnimationName;

    public RuntimeAnimatorController WeaponAnimator => weaponAnimator;
    public WeaponDataSO WeaponData => weaponData;
    public string DashAnimationName => dashAnimationName;

    public abstract void Attack();
    public abstract void Skill();
}

