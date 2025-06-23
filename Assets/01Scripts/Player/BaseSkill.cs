using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour, IActiveSkill
{
    [SerializeField] protected SkillData skillData;

    protected bool isCooldown = false;

    public abstract void Activate(GameObject attacker);
    
    protected bool TryUseSkill()
    {
        if (isCooldown)
            return false;

        if (PlayerStatManager.instance == null)
            return false;

        if( PlayerStatManager.instance.CurrentEP < skillData.manaCost)
            return false;

        if(PlayerStatManager.instance.CurrentEP >= skillData.manaCost)
        {
            PlayerStatManager.instance.ConsumeEP(skillData.manaCost);
            return true;
        }
        return false;
    }

    protected IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        yield return YieldInstructionCache.WaitForSeconds(skillData.cooldown);
        isCooldown = false;
    }
}
