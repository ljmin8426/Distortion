using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillUIManager : MonoBehaviour
{
    [Header("Skill UI Slots")]
    [SerializeField] private List<SkillCooldownUI> skillUISlots;

    private List<SkillBase> acquiredSkills = new List<SkillBase>();

    private int GetSkillSlotIndexByType(SKILL_TYPE type)
    {
        switch (type)
        {
            case SKILL_TYPE.Defense: return 1;
            case SKILL_TYPE.Normal: return 2;
            case SKILL_TYPE.Ultimate: return 3;
            default: return -1;
        }
    }

    public void SetEquipmentSkill(SkillBase newSkill)
    {
        if (newSkill == null) return;

        int index = GetSkillSlotIndexByType(newSkill.SkillType);

        if (index < 1 || index >= skillUISlots.Count)
        {
            Debug.LogWarning("스킬 타입에 맞는 UI 슬롯이 없습니다.");
            return;
        }

        skillUISlots[index].Clear();         // 기존 UI 정리
        skillUISlots[index].Bind(newSkill);  // 새 UI 등록
    }
}
