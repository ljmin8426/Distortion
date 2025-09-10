using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool isOpen = !inventoryView.rootPanel.activeSelf;
            inventoryView.ShowInventory();
            if (isOpen) presenter.RefreshUI();
        }
    }
}

