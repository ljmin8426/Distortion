using System.Collections;
using UnityEngine;

public class ShieldSkill : SkillBase
{
    [Header("Shield Settings")]
    [SerializeField] private float shieldDuration = 5f;
    [SerializeField] private int shieldAmount = 100;

    private GameObject activeEffect;

    public override void Activate(GameObject attacker)
    {
        if (!TryUseSkill())
            return;
        var shield = attacker.GetComponent<Shield>();
        if (shield == null)
            shield = attacker.gameObject.AddComponent<Shield>();

        if (shield.IsShieldActive())
        {
            Debug.Log("방어막이 이미 활성화되어 있습니다.");
            return;
        }

        shield.EnableShield(shieldAmount);

        if (skillData.effectPrefab != null)
        {
            activeEffect = Instantiate(skillData.effectPrefab, attacker.transform);
            activeEffect.transform.localPosition = Vector3.zero;
        }

        StartCoroutine(ShieldDurationRoutine(shield));
        StartCoroutine(base.CooldownRoutine()); // base 호출로 이벤트 호출 권한 유지
    }

    private IEnumerator ShieldDurationRoutine(Shield shield)
    {
        yield return YieldInstructionCache.WaitForSeconds(shieldDuration);

        shield.DisableShield();

        if (activeEffect != null)
        {
            Destroy(activeEffect);
            activeEffect = null;
        }
    }
}
