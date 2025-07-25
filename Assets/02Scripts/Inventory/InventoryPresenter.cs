using UnityEngine;

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
        // 1. 인벤토리 슬롯 갱신
        view.inventoryItemPanelView.ClearAllSlots();

        foreach (var item in model.ownedItems)
        {
            view.inventoryItemPanelView.AddItemSlot(item);
        }

        // 2. 장비 슬롯 갱신
        foreach (var slot in view.equipmentPanelView.equipSlots)
        {
            if (model.equippedItems.TryGetValue(slot.itemType, out var equipped))
                slot.SetItem(equipped);
            else
                slot.Clear();
        }

        // 3. 아이템 정보 갱신
        if (model.selectedItem != null)
        {
            view.itemInfoPanelView.ShowItemInfo(model.selectedItem);
        }
        else
        {
            view.itemInfoPanelView.Hide();
        }
    }

    public void OnItemClick(ItemSO item, InventoryItemSlotView slotView)
    {
        model.SelectItem(item);
        view.itemInfoPanelView.ShowItemInfo(item);
        slotView.Highlight(true);
    }

    public void OnPickupItem(ItemSO item)
    {
        model.ownedItems.Add(item);
        RefreshUI();
    }

    public void OnItemEquip(ItemSO item, EquipSlotView slot)
    {
        if (item is not EquipmentItem equipment)
            return;

        // 기존 장비 해제
        if (slot.CurItem is EquipmentItem oldEquip)
        {
            model.AddItem(oldEquip);
            PlayerStatManager.instance.UnEquip(oldEquip);
        }

        slot.SetItem(equipment);
        model.EquipItem(equipment);
        model.ownedItems.Remove(equipment);
        PlayerStatManager.instance.Equip(equipment);

        RefreshUI();
    }

    public void OnItemUnequip(ITEM_TYPE itemType)
    {
        if (!model.equippedItems.TryGetValue(itemType, out var item))
            return;

        model.UnequipItem(itemType);
        model.AddItem(item);

        if (item is EquipmentItem equipment)
            PlayerStatManager.instance.UnEquip(equipment);

        RefreshUI();
    }

    public void OnUseItem(ItemSO item)
    {
        model.ownedItems.Remove(item);
        RefreshUI();
    }
}