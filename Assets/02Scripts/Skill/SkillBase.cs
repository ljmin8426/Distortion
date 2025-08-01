using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour, IActiveSkill
{
    [SerializeField] protected SkillData skillData;

    protected bool isCooldown = false;

    public event Action<float> OnCooldownStart; // 쿨다운 시작 이벤트
    public SkillData SkillData => skillData;
    public abstract void Activate(GameObject attacker);
    
    protected bool TryUseSkill()
    {
        if (isCooldown)
            return false;

        if (PlayerStatManager.Instance == null)
            return false;

        if( PlayerStatManager.Instance.CurrentEP < skillData.manaCost)
            return false;

        if(PlayerStatManager.Instance.CurrentEP >= skillData.manaCost)
        {
            PlayerStatManager.Instance.ConsumeEP(skillData.manaCost);
            return true;
        }
        return false;
    }

    protected virtual IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        OnCooldownStart?.Invoke(skillData.cooldown);

        yield return YieldInstructionCache.WaitForSeconds(skillData.cooldown);
        isCooldown = false;
    }

}
