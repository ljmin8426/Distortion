using System.Collections.Generic;

public class InventoryModel
{
    public List<ItemDataSO> ownedItems = new();
    public Dictionary<Item_Type, ItemDataSO> equippedItems = new();
    public ItemDataSO selectedItem;

    public void SelectItem(ItemDataSO item) => selectedItem = item;

    public void EquipItem(ItemDataSO item)
    {
        if (item.itemType == Item_Type.Equipment || item.itemType == Item_Type.Skill)
        {
            equippedItems[item.itemType] = item;
            ownedItems.Remove(item);
        }
    }

    public void UnequipItem(Item_Type itemType)
    {
        if (equippedItems.TryGetValue(itemType, out var item))
        {
            AddItem(item);
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
