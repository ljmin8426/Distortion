using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [Header("Sub Views")]
    public StatPanelView statPanelView;
    public EquipmentPanelView equipmentPanelView;
    public ItemInfoPanelView itemInfoPanelView;
    public InventoryItemPanelView inventoryItemPanelView;

    private bool isOpen = false;    

    private void Awake()
    {
        gameObject.transform.localScale = Vector3.zero;
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
            gameObject.transform.localScale = Vector3.one;
        else
            gameObject.transform.localScale = Vector3.zero;
    }
}
