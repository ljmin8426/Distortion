using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentPanelView : MonoBehaviour
{
    public List<EquipSlotView> equipSlots;

    public void SetEquippedItem(ItemDataSO item)
    {
        var slot = equipSlots.FirstOrDefault(s => s.itemType == item.itemType);
        if (slot != null) slot.SetItem(item);
    }

    public void ClearEquippedItem(Item_Type itemType)
    {
        var slot = equipSlots.FirstOrDefault(s => s.itemType == itemType);
        if (slot != null) slot.Clear();
    }
}
