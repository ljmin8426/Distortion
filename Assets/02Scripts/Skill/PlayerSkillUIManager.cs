using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillUIManager : MonoBehaviour
{
    [SerializeField] private List<SkillCooldownUI> skillUISlots;

    private void OnEnable()
    {
        BaseSkillManager.OnSkillEquipped += HandleSkillEquipped;
    }

    private void OnDisable()
    {
        BaseSkillManager.OnSkillEquipped -= HandleSkillEquipped;
    }

    private void Awake()
    {
        for(int i = 0;  i < skillUISlots.Count; i++)
        {
            skillUISlots[i].Clear();
        }
    }

    private void HandleSkillEquipped(SkillBase newSkill)
    {
        if (newSkill == null) return;

        // 순서대로 UI 슬롯에 바인딩
        for (int i = 0; i < skillUISlots.Count; i++)
        {
            if (!skillUISlots[i].IsBound) // 아직 스킬이 바인딩되지 않은 슬롯 찾기
            {
                skillUISlots[i].Bind(newSkill);
                break;
            }
        }
    }
}
