using System.Collections;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;

    public WeaponData WeaponData => weaponData;
    public RuntimeAnimatorController WeaponAnimator => weaponAnimator;
    public string Name => weaponData.weaponName;
    public float AttackDamage => weaponData.attackDamage;
    public float AttackSpeed => weaponData.attackSpeed;


    public abstract void Attack();
}
