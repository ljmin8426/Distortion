using UnityEngine;

public class WarriorSkillManager : BaseSkillManager
{
    protected override void InitializeSkills()
    {
        curSkill = gameObject.GetComponent<GrenadeSkill>();
        skills.Add(curSkill);
    }
}
