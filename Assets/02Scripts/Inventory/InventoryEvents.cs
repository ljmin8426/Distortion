using System;
public static class InventoryEvents
{
    public static Action<ItemDataSO, EquipSlotView> OnEquipItem;
    public static Action<Item_Type> OnUnequipItem;
    public static Action<ItemDataSO, InventoryItemSlotView> OnItemClick;

    public static Action<ItemDataSO> OnUseItem; // 포션 사용

    public static Action<ItemDataSO> OnPickupItem;
}
