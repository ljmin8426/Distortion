using UnityEngine;

public class PlayerSkillUIManager : MonoBehaviour
{
    [SerializeField] private SkillCooldownUI utilityUI;
    [SerializeField] private SkillCooldownUI attackUI;
    [SerializeField] private SkillCooldownUI ultimateUI;
    [SerializeField] private SkillCooldownUI deffenseUI;

    [SerializeField] private SkillBase attackSkill;
    [SerializeField] private SkillBase ultimateSkill;
    [SerializeField] private SkillBase deffenseSkill;

    private void Start()
    {
        if (attackSkill != null) attackUI.Bind(attackSkill);
        if (ultimateSkill != null) ultimateUI.Bind(ultimateSkill);
        if (deffenseSkill != null) deffenseUI.Bind(deffenseSkill);

        // Dash 쿨타임 이벤트 바인딩
        PlayerController.OnDashCooldownStart += utilityUI.StartCooldown;
        utilityUI.SetEpCost(2);
    }

    private void OnDestroy()
    {
        PlayerController.OnDashCooldownStart -= utilityUI.StartCooldown;
    }

    public void SetAttackSkill(SkillBase newSkill)
    {
        attackSkill = newSkill;

        if (newSkill != null)
            attackUI.Bind(newSkill);
        else
            attackUI.Clear(); // UI에서 아이콘 제거
    }
}