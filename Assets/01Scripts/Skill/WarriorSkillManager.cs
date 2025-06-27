using UnityEngine;

public class WarriorSkillManager : BaseSkillManager
{
    [Header("스킬 컴포넌트")]
    [SerializeField] private GrenadeSkill grenadeSkill;

    [Header("스킬 쿨타임 UI")]
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
