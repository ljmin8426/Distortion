using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillCooldownUI : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay;

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
}
