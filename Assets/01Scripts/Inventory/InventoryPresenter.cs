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
            view.itemInfoPanelView.Hide(); // 정보창 초기화 필요시
        }

        // 4. 스탯 패널 등 추가 갱신 필요시 여기에
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
        if (item is not EquipmentItemT equipment)
            return;

        // 기존 장비 해제
        if (slot.CurItem is EquipmentItemT oldEquip)
        {
            model.AddItem(oldEquip);
            PlayerStatManager.instance.Unequip(oldEquip);
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

        if (item is EquipmentItemT equipment)
            PlayerStatManager.instance.Unequip(equipment);

        RefreshUI();
    }

    public void OnUseItem(ItemSO item)
    {
        model.ownedItems.Remove(item);
        RefreshUI();
    }


    public void OnItemDroppedToEquipSlot(ItemSO item, EquipSlotView slotView)
    {
        // 1. 장비 타입이 맞는지 검사
        if (item is EquipmentItemT equipment && equipment.itemType == slotView.itemType)
        {
            // 2. 기존 장비 있으면 우선 해제
            var prevItem = slotView.CurItem;
            if (prevItem is EquipmentItemT prevEquip)
            {
                model.ownedItems.Add(prevEquip);
                PlayerStatManager.instance.Unequip(prevEquip);
            }

            // 3. 슬롯에 장착
            slotView.SetItem(equipment);

            // 4. 인벤토리에서 제거
            model.ownedItems.Remove(equipment);

            // 5. 스탯 적용
            PlayerStatManager.instance.Equip(equipment);

            RefreshUI();
        }
        else
        {
            Debug.Log("타입이 맞지 않아 장착할 수 없습니다.");
        }
    }

}

