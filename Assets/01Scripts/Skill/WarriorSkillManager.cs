using UnityEngine;

public class WarriorSkillManager : BaseSkillManager
{
    [Header("스킬 컴포넌트")]
    [SerializeField] private GrenadeSkill grenadeSkill;
    //[SerializeField] private DashSkill dashSkill;
    //[SerializeField] private EMPBlastSkill empSkill;

    [Header("스킬 쿨타임 UI")]
    [SerializeField] private SkillCooldownUI grenadeSkillUI;
    //[SerializeField] private SkillCooldownUI dashSkillUI;
    //[SerializeField] private SkillCooldownUI empSkillUI;

    protected override void InitializeSkills()
    {
        if (grenadeSkill != null)
        {
            grenadeSkill.SetCooldownUI(grenadeSkillUI);
            skills.Add(grenadeSkill);
        }

        //if (dashSkill != null)
        //{
        //    dashSkill.SetCooldownUI(dashSkillUI);
        //    skills.Add(dashSkill);
        //}

        //if (empSkill != null)
        //{
        //    empSkill.SetCooldownUI(empSkillUI);
        //    skills.Add(empSkill);
        //}
    }
}
