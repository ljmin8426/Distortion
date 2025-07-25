using System.Collections.Generic;

public class InventoryModel
{
    public List<ItemSO> ownedItems = new();
    public Dictionary<ITEM_TYPE, ItemSO> equippedItems = new();
    public ItemSO selectedItem;

    public void SelectItem(ItemSO item) => selectedItem = item;

    public void EquipItem(ItemSO item)
    {
        if (item.itemType == ITEM_TYPE.Equipment || item.itemType == ITEM_TYPE.Passive || item.itemType == ITEM_TYPE.Skill)
        {
            equippedItems[item.itemType] = item;
            ownedItems.Remove(item);
        }
    }

    public void UnequipItem(ITEM_TYPE itemType)
    {
        if (equippedItems.TryGetValue(itemType, out var item))
        {
            AddItem(item); // 다시 인벤토리에 추가
            equippedItems.Remove(itemType);
        }
    }

    public void AddItem(ItemSO item)
    {
        if (!ownedItems.Contains(item))
            ownedItems.Add(item);
    }

    public void RemoveItem(ItemSO item)
    {
        if (ownedItems.Contains(item))
            ownedItems.Remove(item);
    }
}
