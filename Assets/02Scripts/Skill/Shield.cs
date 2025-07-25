using UnityEngine;

public class Shield : MonoBehaviour
{
    private int shieldHp = 0;
    private bool isActive = false;

    public void EnableShield(int amount)
    {
        shieldHp = amount;
        isActive = true;
    }

    public void DisableShield()
    {
        isActive = false;
        shieldHp = 0;
    }

    public int AbsorbDamage(int damage)
    {
        if (!isActive)
        {
            return damage;
        }

        shieldHp -= damage;

        if (shieldHp <= 0)
        {
            DisableShield();
        }

        return Mathf.Max(-shieldHp, 0); // 남은 데미지 반환
    }

    public bool IsShieldActive()
    {
        return isActive;
    }
}
