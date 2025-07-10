using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private InventoryView inventoryView;
    [Header("테스트 아이템들")]
    public List<ItemSO> testItems; // 인스펙터에 드래그해서 넣기

    private InventoryModel model;
    private InventoryPresenter presenter;
    public StatPanelView statPanelView;

    private void Awake()
    {
        model = new InventoryModel();
        presenter = new InventoryPresenter(model, inventoryView);

        // 이벤트 구독
        InventoryEvents.OnItemClick += presenter.OnItemClick;
        InventoryEvents.OnEquipItem += presenter.OnItemEquip;
        InventoryEvents.OnUnequipItem += presenter.OnItemUnequip;
        InventoryEvents.OnUseItem += presenter.OnUseItem;
        InventoryEvents.OnPickupItem += presenter.OnPickupItem;
    }

    private void OnDestroy()
    {
        // 이벤트 해제
        InventoryEvents.OnItemClick -= presenter.OnItemClick;
        InventoryEvents.OnEquipItem -= presenter.OnItemEquip;
        InventoryEvents.OnUnequipItem -= presenter.OnItemUnequip;
        InventoryEvents.OnUseItem -= presenter.OnUseItem;
        InventoryEvents.OnPickupItem -= presenter.OnPickupItem;
    }

    private void Start()
    {
        statPanelView.Bind(PlayerStatManager.instance);
        // 테스트 아이템 넣기
        foreach (var item in testItems)
        {
            model.AddItem(item);
        }
        presenter.RefreshUI();
        inventoryView.ShowInventory(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isOpen = !inventoryView.rootPanel.activeSelf;
            inventoryView.ShowInventory(isOpen);
            if (isOpen) presenter.RefreshUI();
        }
    }
}

