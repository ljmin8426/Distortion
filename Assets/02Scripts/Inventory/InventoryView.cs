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

    private bool isOpen = false;

    private void Awake()
    {
        rootPanel.transform.localScale = Vector3.zero; // 시작 시 꺼진 상태
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;

        if (isOpen)
            rootPanel.transform.localScale = Vector3.one;
        else
            rootPanel.transform.localScale = Vector3.zero;
    }
}
