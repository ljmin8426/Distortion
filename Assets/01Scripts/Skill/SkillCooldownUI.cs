using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SkillCooldownUI : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI epCostText;

    public void Bind(SkillBase skill)
    {
        if (skill == null) return;

        // 스킬 아이콘 초기화
        if (iconImage != null)
            iconImage.sprite = skill.SkillData.SkillIcon;

        if (skill != null && skill.SkillData != null && epCostText != null)
        {
            epCostText.text = $"{skill.SkillData.manaCost}";
        }

        // 쿨다운 시작 이벤트 구독
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
        if (cooldownOverlay != null)
            cooldownOverlay.fillAmount = 0f;
        if (iconImage != null)
            iconImage.sprite = null;
        if (epCostText != null)
            epCostText.text = "";
    }

}
