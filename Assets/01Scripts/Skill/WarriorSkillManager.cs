using UnityEngine;

public class WarriorSkillManager : BaseSkillManager
{
    [SerializeField] private GrenadeSkill grenadeSkill;
    [SerializeField] private SkillCooldownUI grenadeSkillUI;

    protected override void InitializeSkills()
    {
        if (grenadeSkill != null)
        {
            grenadeSkill.SetCooldownUI(grenadeSkillUI);
            skills.Add(grenadeSkill);
        }
    }
}
