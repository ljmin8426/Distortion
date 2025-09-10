using UnityEngine;

public class SkillController : BaseSkillManager
{
    private void OnEnable()
    {
        PlayerInputManager.OnSkillQ += HandleQSkill;
        PlayerInputManager.OnSkillE += HandleESkill;
    }

    private void OnDisable()
    {
        PlayerInputManager.OnSkillQ -= HandleQSkill;
        PlayerInputManager.OnSkillE -= HandleESkill;
    }

    private void HandleQSkill()
    {
        UseSkill(0);
    }

    private void HandleESkill()
    {
        UseSkill(1);
    }
    protected override void InitializeSkills()
    {
        
    }
}
