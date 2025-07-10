using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemSO itemData;

    public ItemSO ItemData => itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 인벤토리에 추가
            InventoryEvents.OnPickupItem?.Invoke(itemData);

            // 오브젝트 제거
            Destroy(gameObject);
        }
    }
}
