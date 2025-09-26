using System.Collections.Generic;
using UnityEngine;

public class InventoryItemPanelView : MonoBehaviour
{
    [SerializeField] private GameObject itemSlotPrefab;

    private List<InventoryItemSlotView> slotViews = new();

    public void ClearAllSlots()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var slot in slotViews)
        {
            Destroy(slot.gameObject);
        }

        slotViews.Clear();
    }

    public void AddItemSlot(ItemDataSO item)
    {
        var obj = Instantiate(itemSlotPrefab, gameObject.transform);
        var slotView = obj.GetComponent<InventoryItemSlotView>();
        slotView.Set(item);
        slotViews.Add(slotView);
    }
}
