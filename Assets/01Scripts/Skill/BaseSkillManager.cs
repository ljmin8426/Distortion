using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillManager : MonoBehaviour
{
    protected List<IActiveSkill> skills = new List<IActiveSkill>();

    protected IActiveSkill curSkill;

    protected void Awake()
    {
        InitializeSkills();
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

    protected abstract void InitializeSkills();
}
