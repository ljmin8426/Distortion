using System.Collections;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;

    public WeaponData WeaponData => weaponData;
    public string Name => weaponData.weaponName;
    public float AttackDamage => weaponData.attackDamage;
    public float AttackSpeed => weaponData.attackSpeed;

    public abstract void Attack();
}

