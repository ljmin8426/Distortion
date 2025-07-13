using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipSlotView : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [Header("슬롯 타입")]
    public ITEM_TYPE itemType;

    [Header("UI 요소")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;

    public ItemSO CurItem { get; private set; }
    public GameObject SpawnedSkillObj { get; private set; }

    /// <summary>
    /// 아이템 장착
    /// </summary>
    public void SetItem(ItemSO item)
    {
        CurItem = item;

        if (icon != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
        }

        if (itemName != null)
            itemName.text = item.itemName;
    }

    /// <summary>
    /// 슬롯 초기화 (스킬 오브젝트 포함)
    /// </summary>
    public void Clear()
    {
        CurItem = null;

        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }

        if (itemName != null)
            itemName.text = "";

        if (SpawnedSkillObj != null)
        {
            Destroy(SpawnedSkillObj);
            SpawnedSkillObj = null;
        }
    }

    /// <summary>
    /// 아이템 드래그 드롭 처리
    /// </summary>
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

    /// <summary>
    /// 우클릭 해제 처리
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && CurItem != null)
        {
            InventoryEvents.OnUnequipItem?.Invoke(itemType);
        }
    }

    /// <summary>
    /// 스폰된 스킬 오브젝트 저장 (외부에서 할당)
    /// </summary>
    public void SetSpawnedSkill(GameObject skillGO)
    {
        if (SpawnedSkillObj != null)
            Destroy(SpawnedSkillObj);

        SpawnedSkillObj = skillGO;
    }
}
