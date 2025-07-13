using UnityEngine;

public class WarriorSkillManager : BaseSkillManager
{
    protected override void InitializeSkills()
    {
        
    }


    public void SetEquipmentSkill(SkillBase newSkill)
    {
        if (newSkill == null) return;

        int index = GetSkillSlotIndexByType(newSkill.SkillData.skillType);

        while (skills.Count <= index)
            skills.Add(null);

        if (skills[index] != null)
            Destroy(((MonoBehaviour)skills[index]).gameObject);

        skills[index] = newSkill;
    }


}
