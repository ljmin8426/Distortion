using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Skill Item")]
public class SkillItemSO : ItemSO
{
    public GameObject skillPrefab; // SkillBase를 포함한 프리팹

    private void OnEnable()
    {
        itemType = ITEM_TYPE.Skill; // 이걸 확실히 지정
    }
}
