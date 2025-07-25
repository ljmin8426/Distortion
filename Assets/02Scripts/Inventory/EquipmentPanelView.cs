using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentPanelView : MonoBehaviour
{
    public List<EquipSlotView> equipSlots;

    public void SetEquippedItem(ItemSO item)
    {
        var slot = equipSlots.FirstOrDefault(s => s.itemType == item.itemType);
        if (slot != null) slot.SetItem(item);
    }

    public void ClearEquippedItem(ITEM_TYPE itemType)
    {
        var slot = equipSlots.FirstOrDefault(s => s.itemType == itemType);
        if (slot != null) slot.Clear();
    }
}
