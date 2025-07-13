using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemSO itemData;

    public ItemSO ItemData => itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        ItemSO grantedItem = Instantiate(itemData); // 원본 훼손 방지용 복제

        // 만약 장비 아이템이라면 등급 랜덤 설정
        if (grantedItem is EquipmentItem equipmentItem)
        {
            equipmentItem.rarity = GetRandomRarity();
        }

        // 인벤토리에 추가
        InventoryEvents.OnPickupItem?.Invoke(grantedItem);

        AudioManager.instance.PlaySFX("Item");

        // 오브젝트 제거
        Destroy(transform.parent.gameObject);
    }

    private ITEM_RARITY GetRandomRarity()
    {
        // 원하는 확률로 설정
        float rand = Random.value;

        if (rand < 0.5f) return ITEM_RARITY.Common;      // 50%
        if (rand < 0.8f) return ITEM_RARITY.Rare;        // 30%
        if (rand < 0.95f) return ITEM_RARITY.Epic;       // 15%
        return ITEM_RARITY.Legendary;                    // 5%
    }
}
