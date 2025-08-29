using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SkillCooldownUI : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI epCostText;

    private SkillBase boundSkill;

    public bool IsBound => boundSkill != null;

    public void Bind(SkillBase skill)
    {
        if (skill == null) return;

        boundSkill = skill;

        // 아이콘 설정
        if (iconImage != null)
            iconImage.sprite = skill.Icon;

        if (skill != null && epCostText != null)
            epCostText.text = $"{skill.ManaCost}";

        // 쿨다운 이벤트 연결
        skill.OnCooldownStart += StartCooldown;
    }

    public void StartCooldown(float cooldown)
    {
        if (cooldownOverlay != null)
            StartCoroutine(CooldownFillRoutine(cooldown));
    }

    private IEnumerator CooldownFillRoutine(float cooldown)
    {
        float timer = 0f;
        cooldownOverlay.fillAmount = 1f;

        while (timer < cooldown)
        {
            timer += Time.deltaTime;
            cooldownOverlay.fillAmount = 1f - (timer / cooldown);
            yield return null;
        }

        cooldownOverlay.fillAmount = 0f;
    }

    public void SetEpCost(int cost)
    {
        if (epCostText != null)
            epCostText.text = $"{cost}";
    }

    public void Clear()
    {
        // 이벤트 해제
        if (boundSkill != null)
            boundSkill.OnCooldownStart -= StartCooldown;

        boundSkill = null;

        if (cooldownOverlay != null)
            cooldownOverlay.fillAmount = 0f;

        if (iconImage != null)
            iconImage.sprite = null;

        if (epCostText != null)
            epCostText.text = "";
    }
}
