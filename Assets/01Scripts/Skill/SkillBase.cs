using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour, IActiveSkill
{
    [SerializeField] protected SkillData skillData;

    protected bool isCooldown = false;
    protected SkillCooldownUI cooldownUI;

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

    public void SetCooldownUI(SkillCooldownUI ui)
    {
        cooldownUI = ui;
    }

    protected IEnumerator CooldownRoutine()
    {
        isCooldown = true;

        if (cooldownUI != null)
            cooldownUI.StartCooldown(skillData.cooldown);

        yield return YieldInstructionCache.WaitForSeconds(skillData.cooldown);
        isCooldown = false;
    }

}
