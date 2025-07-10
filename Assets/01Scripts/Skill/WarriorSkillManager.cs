using UnityEngine;

public class WarriorSkillManager : BaseSkillManager
{
    public HomingMissileSkill attackSkill;

    protected override void InitializeSkills()
    {
        if (attackSkill != null)
        {
            skills.Add(attackSkill);
        }
    }
}
