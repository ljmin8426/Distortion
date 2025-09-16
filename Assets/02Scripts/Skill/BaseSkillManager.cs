using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillManager : MonoBehaviour
{
    protected List<IActiveSkill> skills = new List<IActiveSkill>();

    public static event Action<SkillBase> OnSkillEquipped;

    [SerializeField] private SkillBase[] skillBase;

    protected virtual void InitializeSkills()
    {
        for (int i = 0; i < skillBase.Length; i++)
        {
            SetEquipmentSkill(skillBase[i]);
        }
    }

    public virtual void UseSkill(int index)
    {
        if (index < 0 || index >= skills.Count) 
            return;

        var skill = skills[index];
        if (skill != null)
        {
            skill.Activate(gameObject);
        }
        else
        {
            Debug.Log("스킬이 존재하지않습니다");
        }
    }

    public virtual void SetEquipmentSkill(SkillBase newSkill)
    {
        if (newSkill == null) return;

        skills.Add(newSkill);

        OnSkillEquipped?.Invoke(newSkill);
    }
}
