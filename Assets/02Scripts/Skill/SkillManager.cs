using UnityEngine;

public class SkillManager : BaseSkillManager
{
    private void OnEnable()
    {
        PlayerInputManager.OnSkill += AttackSkill;
        PlayerInputManager.OnDefense += HandleDefense;
        PlayerInputManager.OnUltimate += HandleUltimate;
    }

    private void OnDisable()
    {
        PlayerInputManager.OnSkill -= AttackSkill;
        PlayerInputManager.OnDefense -= HandleDefense;
        PlayerInputManager.OnUltimate -= HandleUltimate;
    }

    private void HandleDefense()
    {
        UseSkill(1);
    }

    private void AttackSkill()
    {
        UseSkill(2);
    }

    private void HandleUltimate()
    {
        UseSkill(3);
    }
    protected override void InitializeSkills()
    { 
        
    }
}
