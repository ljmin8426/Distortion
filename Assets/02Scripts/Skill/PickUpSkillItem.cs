using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickUpSkillItem : MonoBehaviour
{
    [SerializeField] private SkillItemSO ItemData;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var skillManager = other.GetComponent<BaseSkillManager>();

        if (ItemData == null || ItemData.skillPrefab == null || skillManager == null) return;

        var existing = other.GetComponentsInChildren<SkillBase>();

        foreach (var e in existing)
        {
            if (e.GetType() == ItemData.skillPrefab.GetComponent<SkillBase>().GetType())
            {
                Debug.Log("이미 동일한 스킬을 보유하고 있음");
                Destroy(gameObject);
                return;
            }
        }

        var skillObj = Instantiate(ItemData.skillPrefab, other.transform);
        var skill = skillObj.GetComponent<SkillBase>();

        if (skill != null)
        {
            skillManager.SetEquipmentSkill(skill);
        }

        Destroy(gameObject);
    }
}
