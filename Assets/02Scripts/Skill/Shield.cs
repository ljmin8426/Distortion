using System;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float shieldHp = 0;
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

    public float AbsorbDamage(float damage)
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

        return Mathf.Max(-shieldHp, 0);
    }

    public bool IsShieldActive()
    {
        return isActive;
    }
}
