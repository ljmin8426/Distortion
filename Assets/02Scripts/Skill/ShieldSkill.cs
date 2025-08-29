using System.Collections;
using UnityEngine;

public class ShieldSkill : SkillBase
{
    [Header("Shield Setting")]
    [SerializeField] private float shieldDuration = 5f;
    [SerializeField] private int shieldAmount = 100;
    [SerializeField] private GameObject effectPrefab;

    private GameObject activeEffect;

    public override void Activate(GameObject attacker)
    {
        if (!TryUseSkill()) return;

        PlayerStatManager.Instance.ConsumeEP(manaCost);

        var shield = attacker.GetComponent<Shield>();

        if (shield == null)
            shield = attacker.gameObject.AddComponent<Shield>();

        if (shield.IsShieldActive())
        {
            Debug.Log("방어막이 이미 활성화되어 있습니다.");
            return;
        }

        shield.EnableShield(shieldAmount);

        if (effectPrefab != null)
        {
            activeEffect = Instantiate(effectPrefab, attacker.transform);
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
