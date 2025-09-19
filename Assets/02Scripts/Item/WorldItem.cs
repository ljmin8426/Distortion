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

        if (grantedItem is EquipmentItem equipmentItem)
        {
            equipmentItem.rarity = GetRandomRarity();
        }

        InventoryEvents.OnPickupItem?.Invoke(grantedItem);

        Destroy(gameObject);
    }

    private Item_Rarity GetRandomRarity()
    {
        float rand = Random.value;

        if (rand < 0.5f) return Item_Rarity.Common;    
        if (rand < 0.8f) return Item_Rarity.Rare;      
        if (rand < 0.95f) return Item_Rarity.Epic;     
        return Item_Rarity.Legendary;                  
    }
}
