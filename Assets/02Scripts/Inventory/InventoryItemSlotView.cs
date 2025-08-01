using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemSlotView : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image itemImage;
    public Image backgroundImage;
    public GameObject selectHighlight;

    public ItemSO Item { get; private set; }

    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void Set(ItemSO item)
    {
        Item = item;
        itemImage.sprite = item.icon;
        itemImage.enabled = true;

        if (item is EquipmentItem equipment)
        {
            backgroundImage.sprite = ItemRaritySpriteUtility.GetBackgroundSprite(equipment.rarity);
        }
        else
        {
            backgroundImage.sprite = ItemRaritySpriteUtility.GetBackgroundSprite(ITEM_RARITY.Common);
        }
    }

    public void Highlight(bool isOn)
    {
        selectHighlight.SetActive(isOn);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryEvents.OnItemClick?.Invoke(Item, this);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item.itemType == ITEM_TYPE.Potion)
            {
                PlayerStatManager stat = PlayerStatManager.Instance;
                if (Item is PotionItem potion)
                {
                    if (potion.restoreHP)
                        stat.RecoverHP(potion.restoreAmount);
                    else
                        stat.RecoverEP(potion.restoreAmount);
                }

                // 아이템 사용 후 인벤토리에서 제거
                InventoryEvents.OnUseItem?.Invoke(Item);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
    }
}
