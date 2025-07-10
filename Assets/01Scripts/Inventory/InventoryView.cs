using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [Header("Inventory Root")]
    public GameObject rootPanel; // 인벤토리 창 전체 ON/OFF용

    [Header("Sub Views")]
    public StatPanelView statPanelView;
    public EquipmentPanelView equipmentPanelView;
    public ItemInfoPanelView itemInfoPanelView;
    public InventoryItemPanelView inventoryItemPanelView;

    public void ShowInventory(bool show)
    {
        rootPanel.SetActive(show);
    }
}
