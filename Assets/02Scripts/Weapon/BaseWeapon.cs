using System.Collections;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected WeaponDataSO weaponData;

    [SerializeField] protected RuntimeAnimatorController weaponAnimator;

    [SerializeField] protected AudioClip attackSound;

    public RuntimeAnimatorController WeaponAnimator => weaponAnimator;
    public WeaponDataSO WeaponData => weaponData;
    public AudioClip AttackSound => attackSound;

    public abstract void Attack();
    public abstract void Skill();
}

