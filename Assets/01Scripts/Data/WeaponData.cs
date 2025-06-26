using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weaopns/Weaopn Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    public WEAPON_TYPE weaponType;

    public int attackDamage;

    public float attackSpeed;
}
