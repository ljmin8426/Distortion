using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemDataSO itemData;

    public ItemDataSO ItemData => itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        ItemDataSO grantedItem = Instantiate(itemData); // 원본 훼손 방지용 복제

        // 만약 장비 아이템이라면 등급 랜덤 설정
        if (grantedItem is EquipmentItem equipmentItem)
        {
            equipmentItem.rarity = GetRandomRarity();
        }

        // 인벤토리에 추가
        InventoryEvents.OnPickupItem?.Invoke(grantedItem);

        // 오브젝트 제거
        Destroy(transform.parent.gameObject);
    }

    private Item_Rarity GetRandomRarity()
    {
        // 원하는 확률로 설정
        float rand = Random.value;

        if (rand < 0.5f) return Item_Rarity.Common;      // 50%
        if (rand < 0.8f) return Item_Rarity.Rare;        // 30%
        if (rand < 0.95f) return Item_Rarity.Epic;       // 15%
        return Item_Rarity.Legendary;                    // 5%
    }
}
