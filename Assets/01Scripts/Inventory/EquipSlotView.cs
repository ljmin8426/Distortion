using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquipSlotView : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public ITEM_TYPE itemType;
    public Image icon;
    public TextMeshProUGUI itemName;

    private ItemSO currentItem;

    public GameObject spawnedSkillObj; // 동적으로 생성된 스킬 저장

    public ItemSO CurItem => currentItem;

    public void SetItem(ItemSO item)
    {
        currentItem = item;
        icon.sprite = item.icon;
        icon.enabled = true;
        itemName.text = item.itemName;
    }

    public void Clear()
    {
        if (spawnedSkillObj != null)
        {
            GameObject.Destroy(spawnedSkillObj);
            spawnedSkillObj = null;
        }
        currentItem = null;
        icon.sprite = null;
        icon.enabled = false;
        itemName.text = "";
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<InventoryItemSlotView>();
        if (dragged == null) return;

        if (dragged.Item.itemType == itemType)
        {
            InventoryEvents.OnEquipItem?.Invoke(dragged.Item, this); // 슬롯 같이 전달
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && currentItem != null)
        {
            InventoryEvents.OnUnequipItem?.Invoke(itemType);
        }
    }
}
