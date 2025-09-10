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

    private void Awake()
    {
        rootPanel.transform.localScale = Vector3.zero;
    }

    public void ShowInventory()
    {
        rootPanel.transform.localScale = Vector3.one;
    }
}
