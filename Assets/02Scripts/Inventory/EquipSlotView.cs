using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipSlotView : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [Header("슬롯 타입")]
    public Item_Type itemType;

    [Header("UI 요소")]
    [SerializeField] private Image icon;

    public ItemDataSO CurItem { get; private set; }
    public GameObject SpawnedSkillObj { get; private set; }

    public void SetItem(ItemDataSO item)
    {
        CurItem = item;

        if (icon != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
        }
    }

    public void Clear()
    {
        CurItem = null;

        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }

        if (SpawnedSkillObj != null)
        {
            Destroy(SpawnedSkillObj);
            SpawnedSkillObj = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggedSlot = eventData.pointerDrag?.GetComponent<InventoryItemSlotView>();
        if (draggedSlot == null) return;

        var droppedItem = draggedSlot.Item;
        if (droppedItem == null) return;

        if (droppedItem.itemType != itemType)
        {
            Debug.LogWarning($"[EquipSlot] 이 슬롯은 {itemType} 타입만 허용됩니다. 현재 아이템 타입: {droppedItem.itemType}");
            return;
        }

        InventoryEvents.OnEquipItem?.Invoke(droppedItem, this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && CurItem != null)
        {
            InventoryEvents.OnUnequipItem?.Invoke(itemType);
        }
    }

    public void SetSpawnedSkill(GameObject skillGO)
    {
        if (SpawnedSkillObj != null)
            Destroy(SpawnedSkillObj);

        SpawnedSkillObj = skillGO;
    }
}
