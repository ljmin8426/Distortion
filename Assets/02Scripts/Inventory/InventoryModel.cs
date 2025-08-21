using System.Collections.Generic;

public class InventoryModel
{
    public List<ItemDataSO> ownedItems = new();
    public Dictionary<ITEM_TYPE, ItemDataSO> equippedItems = new();
    public ItemDataSO selectedItem;

    public void SelectItem(ItemDataSO item) => selectedItem = item;

    public void EquipItem(ItemDataSO item)
    {
        if (item.itemType == ITEM_TYPE.Equipment || item.itemType == ITEM_TYPE.Skill)
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

    public void AddItem(ItemDataSO item)
    {
        if (!ownedItems.Contains(item))
            ownedItems.Add(item);
    }

    public void RemoveItem(ItemDataSO item)
    {
        if (ownedItems.Contains(item))
            ownedItems.Remove(item);
    }
}
