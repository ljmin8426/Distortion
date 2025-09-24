using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour, IActiveSkill
{
    [Header("Skill Setting")]
    [SerializeField] protected Skill_Type skillType;
    [SerializeField] protected int cooldown;
    [SerializeField] protected int epCost;
    [SerializeField] protected Sprite icon;

    protected bool isCooldown = false;

    public event Action<float> OnCooldownStart;

    public int EPCost => epCost;
    public int Cooldown => cooldown;
    public Sprite Icon => icon;
    public Skill_Type SkillType => skillType;

    public abstract void Activate(GameObject attacker);
    
    protected bool TryUseSkill()
    {
        if (isCooldown) return false;

        if (PlayerStatManager.Instance == null) return false;

        if( PlayerStatManager.Instance.CurrentEP < epCost) return false;

        if(PlayerStatManager.Instance.CurrentEP >= epCost)
        {
            PlayerStatManager.Instance.ConsumeEP(epCost);
            return true;
        }

        return false;
    }

    protected virtual IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        OnCooldownStart?.Invoke(cooldown);

        yield return YieldCache.WaitForSeconds(cooldown);
        isCooldown = false;
    }

}
