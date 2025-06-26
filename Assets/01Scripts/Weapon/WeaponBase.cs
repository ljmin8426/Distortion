using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] protected WeaponData weaponData;
    public WeaponData Data => weaponData;

    protected bool isAttacking = false;
    public bool CanAttack => !isAttacking;

    protected virtual void Awake()
    {
        // 무기 공통 초기화 
    }

    protected virtual bool TryAttack()
    {
        if (isAttacking) 
            return false;

        if (!isAttacking)
        {
            isAttacking = true;
            return true;
        }

        return false;
    }

    public abstract void Attack(); // 무기마다 다른 공격

    protected virtual IEnumerator AttackDelay()
    {
        yield return YieldInstructionCache.WaitForSeconds(weaponData.attackSpeed);
        isAttacking = false;
    }

    public virtual void OnEquip() { }

    public virtual void OnUnequip() { }
}
