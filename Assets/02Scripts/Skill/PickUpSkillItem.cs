using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickUpSkillItem : MonoBehaviour
{
    [SerializeField] private SkillItemSO ItemData;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var skillManager = other.GetComponent<BaseSkillManager>();
        var uiManager = FindAnyObjectByType<PlayerSkillUIManager>();

        if (ItemData == null || ItemData.skillPrefab == null || skillManager == null) return;

        // 이미 같은 타입의 스킬이 존재하는지 검사
        var existing = other.GetComponentsInChildren<SkillBase>();

        foreach (var e in existing)
        {
            if (e.GetType() == ItemData.skillPrefab.GetComponent<SkillBase>().GetType())
            {
                Debug.Log("이미 동일한 스킬을 보유하고 있음");
                Destroy(transform.parent.gameObject);
                return;
            }
        }

        // 스킬 생성 및 추가
        var skillObj = Instantiate(ItemData.skillPrefab, other.transform);
        var skill = skillObj.GetComponent<SkillBase>();

        if (skill != null)
        {
            skillManager.SetEquipmentSkill(skill);    // BaseSkillManager 리스트에 추가
            uiManager?.SetEquipmentSkill(skill);      // UI에 등록
        }

        Destroy(transform.parent.gameObject);
    }
}
