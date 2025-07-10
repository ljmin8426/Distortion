using System;
public static class InventoryEvents
{
    public static Action<ItemSO, EquipSlotView> OnEquipItem;
    public static Action<ITEM_TYPE> OnUnequipItem;
    public static Action<ItemSO, InventoryItemSlotView> OnItemClick;

    public static Action<ItemSO> OnUseItem; // 포션 사용

    public static Action<ItemSO> OnPickupItem;
}
