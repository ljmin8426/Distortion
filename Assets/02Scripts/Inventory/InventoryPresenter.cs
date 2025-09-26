public class InventoryPresenter
{
    private readonly InventoryModel model;
    private readonly InventoryView view;

    public InventoryPresenter(InventoryModel model, InventoryView view)
    {
        this.model = model;
        this.view = view;
    }

    public void RefreshUI()
    {
        view.inventoryItemPanelView.ClearAllSlots();

        foreach (var item in model.ownedItems)
        {
            view.inventoryItemPanelView.AddItemSlot(item);
        }

        foreach (var slot in view.equipmentPanelView.equipSlots)
        {
            if (model.equippedItems.TryGetValue(slot.itemType, out var equipped))
                slot.SetItem(equipped);
            else
                slot.Clear();
        }

        if (model.selectedItem != null)
        {
            view.itemInfoPanelView.ShowItemInfo(model.selectedItem);
        }
        else
        {
            view.itemInfoPanelView.Hide();
        }
    }

    public void OnItemClick(ItemDataSO item, InventoryItemSlotView slotView)
    {
        model.SelectItem(item);
        view.itemInfoPanelView.ShowItemInfo(item);
    }

    public void OnPickupItem(ItemDataSO item)
    {
        model.ownedItems.Add(item);
        RefreshUI();
    }

    public void OnItemEquip(ItemDataSO item, EquipSlotView slot)
    {
        if (item is not EquipmentItem equipment)
            return;

        // 기존 장비 해제
        if (slot.CurItem is EquipmentItem oldEquip)
        {
            model.AddItem(oldEquip);
            PlayerStatManager.Instance.UnEquip(oldEquip);
        }

        slot.SetItem(equipment);
        model.EquipItem(equipment);
        model.ownedItems.Remove(equipment);
        PlayerStatManager.Instance.Equip(equipment);

        RefreshUI();
    }

    public void OnItemUnequip(Item_Type itemType)
    {
        if (!model.equippedItems.TryGetValue(itemType, out var item))
            return;

        model.UnequipItem(itemType);
        model.AddItem(item);

        if (item is EquipmentItem equipment)
            PlayerStatManager.Instance.UnEquip(equipment);

        RefreshUI();
    }

    public void OnUseItem(ItemDataSO item)
    {
        model.ownedItems.Remove(item);
        RefreshUI();
    }
}