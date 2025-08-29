using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillManager : MonoBehaviour
{
    protected List<IActiveSkill> skills = new List<IActiveSkill>();

    protected void Awake()
    {
        InitializeSkills();
    }

    protected abstract void InitializeSkills();

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
    
    protected int GetSkillSlotIndexByType(SKILL_TYPE type)
    {
        switch (type)
        {
            case SKILL_TYPE.Defense: return 1;  
            case SKILL_TYPE.Normal: return 2;   
            case SKILL_TYPE.Ultimate: return 3; 
            default: return skills.Count;
        }
    }

    public virtual void SetEquipmentSkill(SkillBase newSkill)
    {
        if (newSkill == null) return;

        int index = GetSkillSlotIndexByType(newSkill.SkillType);

        while (skills.Count <= index)
            skills.Add(null);

        if (skills[index] != null)
            Destroy(((MonoBehaviour)skills[index]).gameObject);

        skills[index] = newSkill;
    }
}
