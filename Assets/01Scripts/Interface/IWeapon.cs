public interface IWeapon
{
    WeaponData Data { get; }

    void Attack();

    bool CanAttack { get; }

    void OnEquip();
    void OnUnequip();
}
