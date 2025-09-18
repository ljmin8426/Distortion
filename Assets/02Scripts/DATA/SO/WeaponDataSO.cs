using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weaopns/Weaopn Data")]
public class WeaponDataSO : ScriptableObject
{
    public string weaponName;

    public Weapon_Type weaponType;

    public int attackDamage;

    public float attackSpeed;
}
