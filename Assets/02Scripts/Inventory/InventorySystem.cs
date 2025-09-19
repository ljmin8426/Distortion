using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private StatPanelView statPanelView;

    private InventoryModel model;
    private InventoryPresenter presenter;

    private void Awake()
    {
        model = new InventoryModel();
        presenter = new InventoryPresenter(model, inventoryView);

        InventoryEvents.OnItemClick += presenter.OnItemClick;
        InventoryEvents.OnEquipItem += presenter.OnItemEquip;
        InventoryEvents.OnUnequipItem += presenter.OnItemUnequip;
        InventoryEvents.OnUseItem += presenter.OnUseItem;
        InventoryEvents.OnPickupItem += presenter.OnPickupItem;
    }

    private void OnDestroy()
    {
        InventoryEvents.OnItemClick -= presenter.OnItemClick;
        InventoryEvents.OnEquipItem -= presenter.OnItemEquip;
        InventoryEvents.OnUnequipItem -= presenter.OnItemUnequip;
        InventoryEvents.OnUseItem -= presenter.OnUseItem;
        InventoryEvents.OnPickupItem -= presenter.OnPickupItem;
    }

    private void Start()
    {
        statPanelView.Bind(PlayerStatManager.Instance);
        presenter.RefreshUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isOpen = !inventoryView.gameObject.activeSelf;
            if (isOpen) 
                presenter.RefreshUI();
        }
    }
}

